using System;
using UnityEngine;
using UnityEngine.UI;
using static AmbientLocationEnum;

[RequireComponent(typeof(Button))]
/// <summary> Класс, представляющий локацию на карте.</summary>
public class Location : MonoBehaviour
{
    public static event Action<LocationType> OnLocationEntered;
    [field: SerializeField]
    /// <summary> Объект панели, которая должна открыться.</summary>
    public GameObject LocationPanel { get; private set; }
    /// <summary> ID локации.</summary>
    public int ID { get; private set; }
    /// <summary> Тип локации.</summary>
    [field: SerializeField] public LocationType locationType { get; private set; }

    /// <summary> Флаг открытия локации.</summary>
    private bool IsOpen = false;

    /// <summary> Переключить панель.</summary>
    public void TogglePanel()
    {
        LocationPanel.SetActive(IsOpen = !IsOpen);
        if (IsOpen)
            OnLocationEntered?.Invoke(locationType);
    }

    /// <summary> Инициализация класса.</summary>
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => AudioManager.Instance.EnterLocation(locationType));
    }
}
