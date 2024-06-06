using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary> Построение линии графика. </summary>
/// <remarks> Использует меш для построения линии. </remarks>
public class UILineRenderer : Graphic
{
    public UIGridRenderer Grid { set { grid = value; } }
    #region Private Fields
    [SerializeField] private float thickness = 10f;
    [SerializeField] private bool center = true;
    [SerializeField] private List<Vector2> points;
    private UIGridRenderer grid;

    private float unitHeight;
    private float unitWidth;
    private float height;
    private float width;
    #endregion

    #region Public Fields
    /// <summary> Добавление новой точки. </summary>
    /// <param name="pos"> Координаты точки. </param>
    public void AddPoint(Vector2 pos)
    {
        points.Add(pos);
    }

    /// <summary> Назначение точек. </summary>
    /// <param name="points"> Список координат точек. </param>
    public void SetPoints(List<Vector2> points)
    {
        this.points.AddRange(points);
        RedrawLine();
    }

    /// <summary> Очистка точек. </summary>
    public void ClearPoints()
    {
        points.Clear();
    }
    #endregion

    #region Protected Methods
    /// <summary> Отрисовка линии. </summary>
    /// <param name="vh"> Вспомогательный класс для построения меша. </param>
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points.Count < 2)
            return;

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;
        unitWidth = width / grid.GridSize.x;
        unitHeight = height / grid.GridSize.y;

        for (int i = 0; i < points.Count - 1; i++)
        {
            //for first point
            float pos1X = points[i].x * unitWidth;
            float pos1Y = points[i].y * unitHeight;
            Vector2 pos1 = new(pos1X, pos1Y);

            //for the second point
            float pos2X = points[i + 1].x * unitWidth;
            float pos2Y = points[i + 1].y * unitHeight;
            Vector2 pos2 = new(pos2X, pos2Y);

            CreateLineSegment(pos1, pos2, vh);

            int index = i * 5;

            // Add the line segment to the triangles array
            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index);

            // These two triangles create the beveled edges
            // between line segments using the end point of
            // the last line segment and the start points of this one
            if (i != 0)
            {
                vh.AddTriangle(index, index - 1, index - 3);
                vh.AddTriangle(index + 1, index - 1, index - 2);
            }
        }
    }
    #endregion

    #region Private Methods
    /// <summary> Создаёт прямоугольник по двум точкам, который потом используется как часть линии. </summary>
    /// <param name="point1"> Начальная точка. </param>
    /// <param name="point2"> Конечная точка. </param>
    /// <param name="vh"> Вспомогательный класс для построения меша. </param>
    private void CreateLineSegment(Vector3 point1, Vector3 point2, VertexHelper vh)
    {

        Vector3 offset = center ? (rectTransform.sizeDelta / 2) : Vector2.zero;

        // Create vertex template
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        // Create the start of the segment
        Quaternion point1Rotation = Quaternion.Euler(0, 0, RotatePointTowards(point1, point2) + 90);
        vertex.position = point1Rotation * new Vector3(-thickness / 2, 0);
        vertex.position += point1 - offset;
        vh.AddVert(vertex);
        vertex.position = point1Rotation * new Vector3(thickness / 2, 0);
        vertex.position += point1 - offset;
        vh.AddVert(vertex);

        // Create the end of the segment
        Quaternion point2Rotation = Quaternion.Euler(0, 0, RotatePointTowards(point2, point1) - 90);
        vertex.position = point2Rotation * new Vector3(-thickness / 2, 0);
        vertex.position += point2 - offset;
        vh.AddVert(vertex);
        vertex.position = point2Rotation * new Vector3(thickness / 2, 0);
        vertex.position += point2 - offset;
        vh.AddVert(vertex);

        // Also add the end point
        vertex.position = point2 - offset;
        vh.AddVert(vertex);
    }

    /// <summary> Получение угла, на который нужно повернуть вершину чтобы она смотрела на другую. </summary>
    /// <param name="vertex"> Вершина, которая поворачивается. </param>
    /// <param name="target"> Вершина, к которой поворачивать. </param>
    /// <returns> Угол, на который нужно повернуть вершину к цели. </returns>
    private float RotatePointTowards(Vector2 vertex, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * (180 / Mathf.PI));
    }

    /// <summary> Перерисовка линии. </summary>
    private void RedrawLine()
    {
        SetVerticesDirty();
    }
    #endregion

}
