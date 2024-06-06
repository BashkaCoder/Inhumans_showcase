using TMPro;
using UnityEngine;


/// <summary> Панель с координатами. </summary>
/// <remarks> Отображение координат на панели. </remarks>
public class GraphTooltip : MonoBehaviour
{
    #region Private Fields
    /// <summary> Компонент текста со значением X. </summary>
    [SerializeField] private TextMeshProUGUI xText;
    /// <summary> Компонент текста со значением Y. </summary>
    [SerializeField] private TextMeshProUGUI yText;
    #endregion

    #region Public Methods
    /// <summary> Назначение координат. </summary>
    /// <param name="coords"> Координаты. </param>
    public void SetCoordsText(Vector2 coords)
    {
        xText.text = coords.x.ToString("0.0");
        yText.text = ((int)coords.y).ToString();
    }
    #endregion
}
