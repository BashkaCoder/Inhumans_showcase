using TMPro;
using UnityEngine;


/// <summary> Отображение значений на осях.</summary>
/// <remarks> Подписывает деления сетки значениями на осях.  </remarks>
public class Labels : MonoBehaviour
{
    #region Public Fields
    /// <summary> Прификс надписи на оси X. </summary>
    public string XPrefix;
    /// <summary> Прификс надписи на оси Y. </summary>
    public string YPrefix;
    #endregion
    #region Private Fields
    /// <summary> Родительский компонент для надписей по оси X. </summary>
    [SerializeField] private Transform xParent;
    /// <summary> Родительский компонент для надписей по оси Y. </summary>
    [SerializeField] private Transform yParent;

    /// <summary> Префаб надписи по оси X. </summary>
    [SerializeField] private RectTransform xLabelPrefab;
    /// <summary> Префаб надписи по оси Y. </summary>
    [SerializeField] private RectTransform yLabelPrefab;

    /// <summary> Отступ по оси X. </summary>
    [SerializeField] private float xOffset;
    /// <summary> Отступ по оси Y. </summary>
    [SerializeField] private float yOffset;
    [SerializeField] private GraphController graphController;
    [SerializeField] private UIGridRenderer grid;
    private RectTransform rectTransform;
    #endregion


    #region MonoBehaviour Methods
    /// <summary> Получение компонентов. </summary>
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    #endregion

    #region Public Methods
    /// <summary> Отображение надписей. </summary>
    /// <param name="yMax"> Максимум графика по оси Y. </param>
    /// <param name="yMin"> Минимум графика по оси Y. </param>
    /// <param name="mode"> Временной промежуток. </param>
    public void DrawLabels(float yMax, float yMin, TimeInterval mode)
    {
        ClearLabels();
        DrawXLabels(mode);
        DrawYLabels(yMax, yMin);
    }
    #endregion

    #region Private Fields
    /// <summary> Отображение надписей по оси X. </summary>
    /// <param name="mode"> Временной промежуток. </param>
    private void DrawXLabels(TimeInterval mode)
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
        switch (mode)
        {
            case TimeInterval.Week:
                DrawWeekLabels();
                break;
            case TimeInterval.Month:
                DrawMonthLabels();
                break;
            case TimeInterval.ThreeMonths:
                DrawThreeMonthLabels();
                break;
            case TimeInterval.SixMonths:
                DrawSixMonthsLabels();
                break;
            default: return;
        }
    }

    /// <summary> Отображение надписей по оси Y. </summary>
    /// <param name="ymax"> Максимальное значение графика по оси Y. </param>
    /// <param name="ymin"> Минимальное значение графика по оси Y. </param>
    private void DrawYLabels(float ymax, float ymin)
    {
        float step = (ymax - ymin) / grid.GridSize.y;
        for (int i = 0; i < grid.GridSize.y + 1; i++)
        {
            RectTransform newLabel = Instantiate(yLabelPrefab, yParent);
            newLabel.anchoredPosition = new Vector2(yOffset, i * rectTransform.rect.height / grid.GridSize.y);
            int value = Mathf.RoundToInt(step * i + ymin);
            newLabel.GetComponent<TextMeshProUGUI>().text = YPrefix + value.ToString();
        }
    }

    /// <summary> Отображение надписей для недельного периода. </summary>
    private void DrawWeekLabels()
    {
        grid.SetGridSize(new Vector2Int(6, grid.GridSize.y));

        for (int i = 0; i < 7; i++)
        {
            RectTransform newLabel = Instantiate(xLabelPrefab, xParent);
            newLabel.anchoredPosition = new Vector2(i * rectTransform.rect.width / grid.GridSize.x, xOffset);
            TextMeshProUGUI textComponent = newLabel.GetComponent<TextMeshProUGUI>();
            textComponent.text = $"{i + 1}";
            //if (i == 6)
            //    textComponent.text = $"{i + 1}, День";
        }
        RectTransform lastLabel = Instantiate(xLabelPrefab, xParent);
        lastLabel.anchoredPosition = new Vector2((6 * rectTransform.rect.width / grid.GridSize.x) + 50f, xOffset);
        lastLabel.GetComponent<TextMeshProUGUI>().text = ", День";
    }

    /// <summary> Отображение надписей для периода в месяц. </summary>
    private void DrawMonthLabels()
    {
        grid.SetGridSize(new Vector2Int(14, grid.GridSize.y));

        for (int i = 0; i < 15; i += 2)
        {
            RectTransform newLabel = Instantiate(xLabelPrefab, xParent);
            newLabel.anchoredPosition = new Vector2(i * rectTransform.rect.width / grid.GridSize.x, xOffset);
            TextMeshProUGUI textComponent = newLabel.GetComponent<TextMeshProUGUI>();
            textComponent.text = $"{i * 2 + 1}";
        }
        RectTransform lastLabel = Instantiate(xLabelPrefab, xParent);
        lastLabel.anchoredPosition = new Vector2((14 * rectTransform.rect.width / grid.GridSize.x) + 50f, xOffset);
        lastLabel.GetComponent<TextMeshProUGUI>().text = ", День";
    }

    /// <summary> Отображение надписей для периода в 3 месяца. </summary>
    private void DrawThreeMonthLabels()
    {
        grid.SetGridSize(new Vector2Int(13, grid.GridSize.y));


        for (int i = 0; i < 13; i += 2)
        {
            RectTransform newLabel = Instantiate(xLabelPrefab, xParent);
            newLabel.anchoredPosition = new Vector2(i * rectTransform.rect.width / grid.GridSize.x, xOffset);
            TextMeshProUGUI textComponent = newLabel.GetComponent<TextMeshProUGUI>();
            textComponent.text = $"{i + 1}";
        }
        RectTransform lastLabel = Instantiate(xLabelPrefab, xParent);
        lastLabel.anchoredPosition = new Vector2((12 * rectTransform.rect.width / grid.GridSize.x) + 65f, xOffset);
        lastLabel.GetComponent<TextMeshProUGUI>().text = ", Неделя";
    }

    /// <summary> Отображение надписей для периода в 6 месяцев. </summary>
    private void DrawSixMonthsLabels()
    {
        grid.SetGridSize(new Vector2Int(6, grid.GridSize.y));

        for (int i = 0; i < 6; i++)
        {
            RectTransform newLabel = Instantiate(xLabelPrefab, xParent);
            newLabel.anchoredPosition = new Vector2(i * rectTransform.rect.width / grid.GridSize.x, xOffset);
            TextMeshProUGUI textComponent = newLabel.GetComponent<TextMeshProUGUI>();
            textComponent.text = $"{i + 1}";
        }
        RectTransform lastLabel = Instantiate(xLabelPrefab, xParent);
        lastLabel.anchoredPosition = new Vector2((5 * rectTransform.rect.width / grid.GridSize.x) + 50f, xOffset);
        lastLabel.GetComponent<TextMeshProUGUI>().text = ", Месяц";
    }

    /// <summary> Очистка надписей. </summary>
    private void ClearLabels()
    {
        ClearLabelsX();
        ClearLabelsY();
    }

    /// <summary> Очистка надписей по оси X. </summary>
    private void ClearLabelsX()
    {
        foreach (Transform child in xParent)
            Destroy(child.gameObject);
    }

    /// <summary> Очистка надписей по оси Y. </summary>
    private void ClearLabelsY()
    {
        foreach (Transform child in yParent)
            Destroy(child.gameObject);
    }
    #endregion
}
