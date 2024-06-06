using System;
using UnityEngine;
using UnityEngine.UI;


/// <summary> Сетка на графике. </summary>
/// <remarks> Рисует меш сетки. </remarks>
public class UIGridRenderer : Graphic
{
    #region Public Fields
    /// <summary> Событие при изменении размера сетки. </summary>
    public event Action OnGridSizeChanged;
    /// <summary> Размер сетки. </summary>
    public Vector2Int GridSize
    {
        get { return gridSize; }
    }
    #endregion

    #region Private Fields
    /// <summary> Толщина линии. </summary>
    [SerializeField] private int thikness;

    /// <summary> Размер сетки. </summary>
    [SerializeField] private Vector2Int gridSize;

    /// <summary> Ширина. </summary>
    private float width;
    /// <summary> Высота. </summary>
    private float height;
    /// <summary> Ширина клетки. </summary>
    private float cellWidth;
    /// <summary> Высота клетки. </summary>
    private float cellHeight;
    #endregion

    #region MonoBehaviour Methods
    /// <summary> Отрисовка сетки при запуске сцены. </summary>
    protected override void Start()
    {
        SetVerticesDirty();
        base.Start();
    }
    #endregion

    #region Public Methods
    /// <summary> Установка размера сетки. </summary>
    /// <param name="newSize"> Новый размер. </param>
    public void SetGridSize(Vector2Int newSize)
    {
        if (newSize == gridSize) return;
        gridSize = newSize;
        OnGridSizeChanged?.Invoke();
        SetVerticesDirty();
    }
    #endregion

    #region Protected Methods
    /// <summary> Отрисовка сетки. </summary>
    /// <param name="vh"> Вспомогательный класс для построения меша. </param>
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        cellWidth = width / gridSize.x;
        cellHeight = height / gridSize.y;

        int count = 0;
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                DrawCell(x, y, count, vh);
                count++;
            }
        }
    }
    #endregion

    #region Private Methods
    /// <summary> Отрисовка клетки. </summary>
    /// <param name="x"> Координата X. </param>
    /// <param name="y"> Координата Y. </param>
    /// <param name="index"> Индекс клетки. </param>
    /// <param name="vh"> Вспомогательный класс для построения меша. </param>
    private void DrawCell(int x, int y, int index, VertexHelper vh)
    {
        float xPos = x * cellWidth;
        float yPos = y * cellHeight;

        int dist = thikness / 2;

        UIVertex vert = UIVertex.simpleVert;
        vert.color = color;

        vert.position = new Vector3(xPos, yPos);
        vh.AddVert(vert);

        vert.position = new Vector3(xPos, yPos + cellHeight);
        vh.AddVert(vert);

        vert.position = new Vector3(xPos + cellWidth, yPos + cellHeight);
        vh.AddVert(vert);

        vert.position = new Vector3(xPos + cellWidth, yPos);
        vh.AddVert(vert);

        vert.position = new Vector3(xPos + dist, yPos + dist);
        vh.AddVert(vert);

        vert.position = new Vector3(xPos + dist, yPos + cellHeight - dist);
        vh.AddVert(vert);

        vert.position = new Vector3(xPos + cellWidth - dist, yPos + cellHeight - dist);
        vh.AddVert(vert);

        vert.position = new Vector3(xPos + cellWidth - dist, yPos + dist);
        vh.AddVert(vert);

        int offset = index * 8;

        vh.AddTriangle(offset + 0, offset + 4, offset + 5);
        vh.AddTriangle(offset + 5, offset + 0, offset + 1);
        vh.AddTriangle(offset + 1, offset + 5, offset + 6);
        vh.AddTriangle(offset + 6, offset + 1, offset + 2);
        vh.AddTriangle(offset + 2, offset + 6, offset + 7);
        vh.AddTriangle(offset + 7, offset + 2, offset + 3);
        vh.AddTriangle(offset + 3, offset + 7, offset + 4);
        vh.AddTriangle(offset + 4, offset + 3, offset + 0);
    }
    #endregion
}
