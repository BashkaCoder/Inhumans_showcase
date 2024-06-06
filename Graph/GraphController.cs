using System.Collections.Generic;
using UnityEngine;


/// <summary> Перечесление временных интервалов. </summary>
public enum TimeInterval
{
    Week,
    Month,
    ThreeMonths,
    SixMonths
}

/// <summary> График. </summary>
/// <remarks> Связующее звено отображения графика, лейблов и сетки. </remarks>
public class GraphController : MonoBehaviour
{
    #region Private Fields
    /// <summary> Надписи значений. </summary>
    [SerializeField] private Labels labels;
    /// <summary> Отображаемый временной промежуток. </summary>
    [SerializeField] private TimeInterval timeMode;
    /// <summary> Сетка. </summary>
    [SerializeField] private UIGridRenderer grid;
    /// <summary> Префаб линии. </summary>
    [SerializeField] private RectTransform linePrefab;
    /// <summary> Максимальное количество отображаемых точек. </summary>
    [SerializeField] private int maxDisplayedPoints;

    [Header("Autosize options")]
    /// <summary> Авторазмер по оси Y. </summary>
    [SerializeField] private bool autosizeYMax;
    /// <summary> Минимальное значение по оси Y. </summary>
    [SerializeField] private bool autosizeYMin;
    /// <summary> Коэффициент максимального значения по оси Y. </summary>
    [SerializeField] private float yMaxMultiplier = 1.2f;
    /// <summary> Коэффициент минимального значения по оси Y. </summary>
    [SerializeField] private float yMinMultiplier = 0.8f;
    /// <summary> Максимальное значение Y по умолчанию. </summary>
    [SerializeField] private float defaultYMax = 120f;
    /// <summary> Минимальное значение Y по умолчанию. </summary>
    [SerializeField] private float defaultYMin = 0f;

    [Header("Grid limitations")]
    /// <summary> Минимальный размер сетки по Y. </summary>
    [SerializeField] private int minGridSizeY;
    /// <summary> Максимальный размер сетки по Y. </summary>
    [SerializeField] private int maxGridSizeY;

    /// <summary> Кнопки изменения временного интервала. </summary>
    [SerializeField] private TimeIntervalButtons timeButtons;
    [SerializeField] private Color[] colors;

    /// <summary> Графики. </summary>
    /// <remarks> Ключ - график, значение - список точек на этом графике. </remarks>
    private Dictionary<UILineRenderer, List<int>> graphs;
    /// <summary> Список отображаемых точек. </summary>
    private List<int> displayedPoints;

    /// <summary> Максимальное значение по Y. </summary>
    private float yMax;
    /// <summary> Минимальное значение по Y. </summary>
    private float yMin;
    public float YMax { get{return yMax;} }
    public float YMin { get{return yMin;} }
    /// <summary> Компонент логики взаимодействия. </summary>
    private GraphInteraction interaction;
    #endregion

    #region MonoBehaviour Methods
    /// <summary> Подписывание методов на события. </summary>
    private void OnEnable()
    {
        timeButtons.OnTimeButtonPressed += ChangeMode;
        grid.OnGridSizeChanged += DrawGraphs;
        timeButtons.OnTimeButtonPressed += DrawLabels;
        TimeManager.OnMinuteChanged += Draw;
    }

    /// <summary> Отписывание методов от событий. </summary>
    private void OnDisable()
    {
        timeButtons.OnTimeButtonPressed -= ChangeMode;
        grid.OnGridSizeChanged -= DrawGraphs;
        timeButtons.OnTimeButtonPressed -= DrawLabels;
        TimeManager.OnMinuteChanged -= Draw;
    }

    /// <summary> Инициализация. </summary>
    private void Awake()
    {
        graphs = new();
        displayedPoints = new();
        interaction = GetComponent<GraphInteraction>();
    }
    #endregion 

    #region Public Methods
    /// <summary> Добавление нового графика. </summary>
    /// <param name="data"> Точки дя графика. </param>
    public void AddNewGraph(List<int> data)
    {
        UILineRenderer newGraph = CreateGraph();
        graphs.Add(newGraph, data);
    }

    /// <summary> Очистка графиков. </summary>
    public void ClearGraphs()
    {
        foreach (UILineRenderer graph in graphs.Keys)
            Destroy(graph.gameObject);
        graphs.Clear();
    }

    /// <summary> Отображение графиков, значений на осях и сетки. </summary>
    public void Draw()
    {
        DrawGraphs();
        grid.SetGridSize(new Vector2Int(grid.GridSize.x, ClampGridSize()));
        DrawLabels(timeMode);
    }

    /// <summary> Отображение значений на осях. </summary>
    /// <param name="_"> Временной интервал. </param>
    private void DrawLabels(TimeInterval _)
    {
        labels.DrawLabels(yMax, yMin, timeMode);
    }

    /// <summary> Отображение графиков. </summary>
    public void DrawGraphs()
    {
        displayedPoints.Clear();
        foreach (UILineRenderer graph in graphs.Keys)
        {
            graph.ClearPoints();
            SetPointsToGraph(graph, graphs[graph]);
        }
    }

    /// <summary> Назначение точек графику. </summary>
    /// <param name="graph"> Точки. </param>
    /// <param name="points"> График. </param>
    public void SetPointsToGraph(UILineRenderer graph, List<int> points)
    {
        List<int> pointsToDisplay = GetLastXPoints(points, GetNumberOfPointsInInterval());
        foreach (int p in pointsToDisplay)
            displayedPoints.Add(p);
        DrawPoints(graph, pointsToDisplay);
    }

    /// <summary> Смена временного режима. </summary>
    /// <param name="newMode"> Новый режим. </param>
    public void ChangeMode(TimeInterval newMode)
    {
        timeMode = newMode;
        DrawGraphs();
    }
    #endregion

    #region Private Methods
    /// <summary> Отображение графика по точкам. </summary>
    /// <param name="graph"> График. </param>
    /// <param name="points"> Точки. </param>
    private void DrawPoints(UILineRenderer graph, List<int> points)
    {
        List<Vector2> cords = new();
        float step = (float)grid.GridSize.x / (GetNumberOfPointsInInterval() - 1);
        float ymax = GetYMax(points);
        float ymin = GetYMin(points);
        for (int i = 0; i < points.Count; i++)
        {
            float x = step * i;
            float y = GetPointY(points[i], ymax, ymin);
            Vector2 position = new(x, y);
            cords.Add(position);
        }
        graph.SetPoints(cords);
        interaction.Step = step;
        interaction.Points = cords;
    }

    /// <summary> Создание нового графика. </summary>
    /// <returns> Новый график. </returns>
    private UILineRenderer CreateGraph()
    {
        RectTransform graph = Instantiate(linePrefab, this.transform, false);

        UILineRenderer renderer = graph.GetComponent<UILineRenderer>();
        renderer.Grid = grid;
        renderer.color = Extensions.GetRandomElement(colors);
        return renderer;
    }

    //returns point y cord in grid units
    /// <summary> Конвертация координат точки. </summary>
    /// <param name="value"> Значение точки. </param>
    /// <param name="yMax"> Минимальное значение по оси Y. </param>
    /// <param name="yMin"> Максимальное значение по оси Y.</param>
    /// <returns></returns>
    private float GetPointY(int value, float yMax, float yMin)
    {
        if (value == 0) return 0f;
        float p = (value - yMin) / (yMax - yMin);
        return p * grid.GridSize.y;
    }

    /// <summary> Получение количества точек во временном интервале. </summary>
    /// <param name="time"> Интервал. </param>
    /// <returns> Количество точек. </returns>
    public int GetNumberOfPointsInInterval()
    {
        return timeMode switch
        {
            TimeInterval.Week => 7,
            TimeInterval.Month => 29,
            TimeInterval.ThreeMonths => 92,
            TimeInterval.SixMonths => 181,
            _ => 7,
        };
    }

    /// <summary> Получение последних x точек. </summary>
    /// <param name="points"> Все точки. </param>
    /// <param name="x"> Х последних точек. </param>
    /// <returns></returns>
    private List<int> GetLastXPoints(List<int> points, int x)
    {
        if (x < points.Count)
            return points.GetRange(points.Count - x, x);

        return points;
    }

    /// <summary> Вычисление максимума графика. </summary>
    /// <param name="points"> Точки. </param>
    /// <returns> Миксимум. </returns>
    private float GetYMax(List<int> points)
    {
        if (autosizeYMax)
            yMax = GetMaxPointValue() * yMaxMultiplier;
        else
            yMax = defaultYMax;
        return yMax;
    }

    /// <summary> Вычисление минимума графика. </summary>
    /// <param name="points"> Точки. </param>
    /// <returns> Минимум. </returns>
    private float GetYMin(List<int> points)
    {
        if (autosizeYMin)
            yMin = GetMinPointValue() * yMinMultiplier;
        else
            yMin = defaultYMin;
        return yMin;
    }

    /// <summary> Вычисление максимальной точки. </summary>
    /// <returns> Максимальная точка. </returns>
    private int GetMaxPointValue()
    {
        int max = 0;
        foreach (int point in displayedPoints)
            if (point > max) max = point;
        return max;
    }

    /// <summary> Вычисление минимальной точки. </summary>
    /// <returns> Минимальная точка. </returns>
    private int GetMinPointValue()
    {
        int min = displayedPoints[0];
        foreach (int point in displayedPoints)
            if (point < min) min = point;
        return min;
    }

    /// <summary> Ограничить размер сетки. </summary>
    /// <returns> Ограниченный размер. </returns>
    private int ClampGridSize()
    {
        int delta = (int)Mathf.Ceil(yMax - yMin);
        int curr = grid.GridSize.y;

        if (delta < maxGridSizeY)
        {
            curr = Mathf.Clamp(curr, minGridSizeY, Mathf.Max(delta, minGridSizeY));
        }
        else
            curr = maxGridSizeY;

        return curr;
    }
    #endregion
}
