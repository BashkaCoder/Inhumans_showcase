using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
/// <summary> Класс, отвечающий за работу кнопки локации на карте.</summary>
public class MapButton : MonoBehaviour
{
    /// <summary> Инициализация класса.</summary>
    public void Initialize()
    {
        GetComponent<Button>().onClick.AddListener(() => AudioManager.Instance.LeaveLocation());
    }
}
