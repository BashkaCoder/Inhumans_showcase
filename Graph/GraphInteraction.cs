using System.Collections.Generic;
using UnityEngine;


/// <summary> Интерактивность графика. </summary>
/// <remarks> Отображение панели с координатами при приближении курсора к графику. </remarks>
public class GraphInteraction : MonoBehaviour
{
    /// <summary> Отображаемые точки. </summary>
    [HideInInspector] public List<Vector2> Points;
    #region Private Fields
    /// <summary> Сетка. </summary>
    [SerializeField] private UIGridRenderer grid;
    /// <summary> Дистанция курсора до графика для отображения панели. </summary>
    [SerializeField] private float tooltipDistance = 1f;
    /// <summary> Всегда рисовать панель. </summary>
    [SerializeField] private bool alwaysDraw;
    /// <summary> Камера. </summary>
    [SerializeField] private Camera mainCamera;
    /// <summary> Префаб панели. </summary>
    [SerializeField] private RectTransform tooltipPrefab;
    [SerializeField] private RectTransform greenDotPrefab;

    /// <summary> Панель. </summary>
    private RectTransform tooltip;
    private RectTransform greenDot;
    /// <summary> Логика панели. </summary>
    private GraphTooltip tooltipSctipt;
    /// <summary> RectTransform компонент. </summary>
    private RectTransform rectTransform;
    private GraphController graphController;
    /// <summary> Шаг между точками. </summary>
    [HideInInspector] public float Step;
    /// <summary> Ширина. </summary>
    private float width;
    /// <summary> Высота. </summary>
    private float height;
    #endregion

    #region MonoBehaviour Methods
    /// <summary> Инициализация. </summary>
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        graphController = GetComponent<GraphController>();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;
    }

    /// <summary> Проверка дистанции курсора до графика. </summary>
    private void Update()
    {
        if (CursorInsideGrid() == false || Points.Count == 0)
        {
            if (tooltip != null) Destroy(tooltip.gameObject);
            if (greenDot != null) Destroy(greenDot.gameObject);
            return; }
        Vector2 pos = GetCursorCoords();
        float percent = pos.x / grid.GridSize.x;
        float index = (graphController.GetNumberOfPointsInInterval()) * percent;
        index = Mathf.Clamp(index, 0, Points.Count - 1);
        int floor = (int)Mathf.Floor(index);
        int ceil = (int)Mathf.Ceil(index);
        float x1 = Points[floor].x;
        float y1 = Points[floor].y;
        float x2 = Points[ceil].x;
        float y2 = Points[ceil].y;
        Vector2 point1 = new(x1, y1);
        Vector2 point2 = new(x2, y2);
        float k = (y2 - y1) / (x2 - x1);
        float b = y2 - k * x2;

        float x = index * Step; // x cursor position in grid units
        float y = k * x + b; // y cursor position in grid units

        float graphX = x / grid.GridSize.x * width;
        float graphY = y / grid.GridSize.y * height;
        Vector2 cursorPos = GetCursorLocalPosition();
        float distance = Vector2.Distance(new Vector2(graphX, graphY), cursorPos);
        if (distance > tooltipDistance && alwaysDraw == false)
        {
            if (tooltip != null) Destroy(tooltip.gameObject);
            if (greenDot != null) Destroy(greenDot.gameObject);
            return;
        }
        float distance1 = Vector2.Distance(pos, point1);
        float distance2 = Vector2.Distance(pos, point2);
        Vector2 snapPos, offset;

        if (tooltip == null) CreateToolTip();
        if (greenDot == null) CreateGreenDot();

        snapPos = distance1 <= distance2 ? point1 : point2;
        tooltipSctipt.SetCoordsText(GridCordsToValues(snapPos));
        snapPos = PosInGridToPx(snapPos);
        offset = GetTooltipOffset(snapPos);
        tooltip.anchoredPosition = snapPos + offset;
        greenDot.anchoredPosition = snapPos;
    }
    #endregion

    #region Private Methods
    /// <summary> Проверка положения курсора. </summary>
    /// <returns> Находится ли курсор внутри сетки. </returns>
    private bool CursorInsideGrid()
    {
        Vector2 cursorPos = GetCursorLocalPosition();
        return cursorPos.x > 0 && cursorPos.x < width
        && cursorPos.y > 0 && cursorPos.y < height;
    }

    private void CreateGreenDot()
    {
        RectTransform gd = Instantiate(greenDotPrefab, rectTransform);
        greenDot = gd;
    }

    /// <summary> Конвертация координат точки. </summary>
    /// <param name="posInGrid"> Точка в координатах сетки. </param>
    /// <returns> Позиция в координатах RectTransform. </returns>
    private Vector2 PosInGridToPx(Vector2 posInGrid)
    {
        return new Vector2(
            posInGrid.x / grid.GridSize.x * width,
            posInGrid.y / grid.GridSize.y * height
        );
    }

    /// <summary> Вычисление отступа панели. </summary>
    /// <param name="tooltipPos"> Позиция панели. </param>
    /// <returns> Отступ. </returns>
    private Vector2 GetTooltipOffset(Vector2 tooltipPos)
    {
        Vector2 offset = Vector2.zero;
        if (tooltipPos.x + tooltipPrefab.rect.width > width)
            offset.x -= tooltipPrefab.rect.width;

        if (tooltipPos.y + tooltipPrefab.rect.height > height)
            offset.y -= tooltipPrefab.rect.height;

        return offset;
    }

    /// <summary> Получение координат курсора. </summary>
    /// <returns> Координаты курсора. </returns>
    private Vector2 GetCursorCoords()
    {
        Vector2 localPosition = GetCursorLocalPosition();

        Vector2 uvCoords = new Vector2(
            Mathf.InverseLerp(0, grid.rectTransform.rect.width, localPosition.x),
            Mathf.InverseLerp(0, grid.rectTransform.rect.height, localPosition.y)
        );

        Vector2 relativeCoords = new Vector2(
            grid.GridSize.x * uvCoords.x,
            grid.GridSize.y * uvCoords.y
        );

        return relativeCoords;
    }

    /// <summary> Получение локальных координат курсора. </summary>
    /// <returns> Локальные координаты. </returns>
    private Vector2 GetCursorLocalPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(grid.rectTransform, mousePosition, mainCamera, out Vector2 localPosition);
        return localPosition;
    }

    /// <summary> Создание панели. </summary>
    private void CreateToolTip()
    {
        RectTransform tt = Instantiate(tooltipPrefab, rectTransform, false);
        tooltip = tt;
        tooltipSctipt = tt.GetComponent<GraphTooltip>();
    }

    private Vector2 GridCordsToValues(Vector2 gridCords)
    {
        GraphController controller = GetComponent<GraphController>();
        float max = controller.YMax;
        float min = controller.YMin;
        float stepY = (max - min) / grid.GridSize.y;
        return new Vector2(
                gridCords.x + 1,
                min + gridCords.y * stepY
        );
    }
    #endregion
}
