using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(WhitehallController))]
/// <summary> Класс управления открытием панели диалога с чиновником.</summary>
public class WhitehallControllerUIHandler : MonoBehaviour
{
    /// <summary> Все кнопки чиновников.</summary>
    [HideInInspector] public Button AlchemyPlacemanButton;
    [HideInInspector] public Button JoineryPlacemanButton;
    [HideInInspector] public Button SmitheryPlacemanButton;

    /// <summary> Панель для вывода диалога.</summary>
    [SerializeField] private WhitehallDetailsUI detailsPanel;

    /// <summary> Метод открытия панели с соответствующим знаком гильдии и процентом выполненных контрактов.</summary>
    public void OpenDetailsPanel(Sprite clanCoatSprite, float trust)
    {
        detailsPanel.RefreshContent(clanCoatSprite, trust);
        detailsPanel.Open();
    }

    /// <summary> Метод открытия панели диалога.</summary>
    public void ShowSpecializationContractsDetails(float trust, Sprite clanCoatSprite)
    {
        OpenDetailsPanel(clanCoatSprite, trust);
    }
}
