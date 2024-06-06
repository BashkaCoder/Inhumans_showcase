using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using static AmbientLocationEnum;

// 0 - Factory
// 1 - Bank
// 2 - WhiteHall
// 3 - Blacksmithers
// 4 - Joiners
// 5 - Alchemists

/// <summary> Класс, отвечающий за работу карты.</summary>
public class Map : MonoBehaviour
{
    /// <summary> Контроллер путей.</summary>
    public RouteController routeController;
    /// <summary> Игрок.</summary>
    [SerializeField] private PlayerMovement player;
    /// <summary> Объект, хранящий локации.</summary>
    [SerializeField] private RectTransform locationsHolder;
    /// <summary> Объект, хранящий канвасы локаций.</summary>
    [SerializeField] private GameObject locationCanvases;

    /// <summary> Словарь индекса к локации.</summary>
    private Dictionary<int, Location> allLocations;
    /// <summary> Текущая локация.</summary>
    private Location currentLocation;
    /// <summary> Индекс предыдущей локации.</summary>
    private int lastVisiteBuildingID = 0;

    /// <summary> Запуск скрипта.</summary>
    private void Start()
    {
        InitializeLocations();
        currentLocation = allLocations[lastVisiteBuildingID];

        //Review => rework
        for (int i = 0; i < locationCanvases.transform.childCount; ++i)
        {
            locationCanvases.transform.GetChild(i).GetComponentInChildren<MapButton>().Initialize();
        }
        //

        AudioManager.Instance.EnterLocation(LocationType.Factory);
        OnBuildingClick(0);

        // Подписываемся на событие прибытия игрока 
        player.OnPlayerArrived += PlayerArrivedHandler;
    }

    /// <summary> Инициализация класса.</summary>
    private void InitializeLocations()
    {
        allLocations = new Dictionary<int, Location>();
        foreach (RectTransform mapChild in locationsHolder)
        {
            if (!mapChild.TryGetComponent<Location>(out Location location))
            {
                Debug.LogError($"Location component not found in mapChild: {mapChild.name}");
                continue;
            }

            ParseLocation(location);
        }
    }

    /// <summary> Спарсить локацию.</summary>
    private void ParseLocation(Location location)
    {
        if (location == null)
        {
            Debug.LogError($"Location object({location.name}) not found!");
            return;
        }

        // Извлекаем инты из имени объекта
        int locationID = ExtractIDFromName(location.name);
        allLocations[locationID] = location;
    }

    /// <summary> Извлечь ID из имени локации.</summary>
    private int ExtractIDFromName(string name)
    {
        // Используем регулярное выражение для поиска числа в конце строки
        Match match = Regex.Match(name, @"\d+");

        // Если найдено число, преобразуем его в int и возвращаем
        if (match.Success)
            return int.Parse(match.Value);

        return -1;
    }

    /// <summary> Обработчик события прибытия игрока.</summary>
    private void PlayerArrivedHandler()
    {
        if (currentLocation == null)
        {
            Debug.LogError($"Current location is null!");
            return;
        }
        gameObject.SetActive(false);
        currentLocation.TogglePanel();
    }

    /// <summary> Событие при нажатии локации.</summary>
    public void OnBuildingClick(int destinationBuildingID)
    {
        if (destinationBuildingID == lastVisiteBuildingID && !player.IsMoving)
        {
            currentLocation.TogglePanel();
            gameObject.SetActive(false);
        }
        else if (!player.IsMoving)
        {
            player.RouteWaypoints = routeController.GetRoute(lastVisiteBuildingID, destinationBuildingID);
            player.MoveToNextTarget();
            lastVisiteBuildingID = destinationBuildingID;
            currentLocation = allLocations[destinationBuildingID];
        }
    }
}
