using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary> Класс управления UI элементами диалога.</summary>
public class WhitehallDetailsUI : MonoBehaviour
{
    /// <summary> Спрайт гильдии.</summary>
    [Header("UI Content")]
    [SerializeField] private Image ClanCoatImage;

    /// <summary> Текст процента выполненных контрактов в пределах [0%; 100%].</summary>
    [SerializeField] private TextMeshProUGUI TrustPercentageText;

    /// <summary> Слайдер процента выполненных контрактов в пределах [0; 1].</summary>
    [SerializeField] private Slider TrustSliderNormalized;

    /// <summary> Ссылка на ConversationManager.</summary>
    [SerializeField] private GameObject conversationManager;
    /// <summary> Ссылка на WhitehallDetails.</summary>
    [SerializeField] private GameObject whitehallDetails;


    /// <summary> Все цвета для разного процента выполненных контрактов.</summary>
    [Header("Color Style")]
    [SerializeField] private Color minColor;
    [SerializeField] private Color midColor;
    [SerializeField] private Color maxColor;

    /// <summary> Открытие объекта, на который прикреплен скрипт.</summary>
    public void Open() => gameObject.SetActive(true);

    /// <summary> Закрытие объекта, на который прикреплен скрипт, с включением всех кнопок</summary>
    public void Close()
    {
        gameObject.SetActive(false);

        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        conversationManager.transform.SetParent(whitehallDetails.transform);
    }

    /// <summary> Метод проверки правильного количества выполненных контрактов и обновление визуала.</summary>
    public void RefreshContent(Sprite clanCoatSprite, float trust)
    {
        if (trust < 0 || trust > 1)
        {
            Debug.LogWarning($"Invalid trust value: {trust}. Trust must be in range[0; 1]!");
            return;
        }

        UpdateTrustVisuals(trust);
        ClanCoatImage.sprite = clanCoatSprite;
    }

    /// <summary> Обновление показаний слайдера.</summary>
    private Image fillImage => TrustSliderNormalized.fillRect.GetComponent<Image>();

    /// <summary> Обновление визуала - текста и спрайта гильдии.</summary>
    private void UpdateTrustVisuals(float trust)
    {
        UpdateTrustColor(trust);
        UpdateTrustText(trust);
        TrustSliderNormalized.value = Mathf.Lerp(TrustSliderNormalized.minValue, TrustSliderNormalized.maxValue, trust);
    }

    /// <summary> Обновление цвета.</summary>
    private void UpdateTrustColor(float trust)
    {
        Color trustColor = GetColorValue(trust);
        fillImage.color = trustColor;
        TrustPercentageText.color = trustColor;
    }

    /// <summary> Обновление текста с процентом выполненных контрактов.</summary>
    private void UpdateTrustText(float trust)
    {
        TrustPercentageText.text = $"{string.Format("{0:F2}", trust * 100)}/100%";
    }

    /// <summary> Выбор цвета.</summary>
    private Color GetColorValue(float t)
    {
        Color resultingColor;
        if (t <= 0.5f)
        {
            resultingColor = Color.Lerp(minColor, midColor, t * 2);
        }
        else
        {
            resultingColor = Color.Lerp(midColor, maxColor, (t - 0.5f) * 2);
        }
        return resultingColor;
    }
}
