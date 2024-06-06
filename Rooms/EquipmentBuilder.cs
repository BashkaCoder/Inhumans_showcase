using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary> Назначение оборудования. </summary>
/// <remarks> Назначает оборудование в комнату, отделяет логику оборудования и выносит её под отдельный объект. </remarks>
public class EquipmentBuilder : MonoBehaviour
{
    /// <summary> Событие при назначении оборудования. </summary>
    public static event Action<StationCraft> OnStationBuilt;
    public static event Action OnEquipmentDestroyed;

    #region Private Fields
    /// <summary> Родительский объект для логики оборудования. </summary>
    [SerializeField] private Transform stationLogicParent;
    /// <summary> Префабы оборудования. </summary>
    [SerializeField] private List<GameObject> prefabs;

    /// <summary> Ключ - объект оборудования, значение - логика производства. </summary>
    private Dictionary<GameObject, StationCraft> stations;
    /// <summary> Ключ - объект оборудования, значение - модель оборудования. </summary>
    private Dictionary<GameObject, StationData> stationsData;
    #endregion

    #region MonoBehaviour Methods
    /// <summary> Подписывание методов на события. </summary>
    private void OnEnable()
    {
        StationMenu.OnRequireStationBuild += BuildEquipment;
        Room.OnDeleteEquipmentButtonClick += DeleteEquipment;
    }

    /// <summary> Отписка методов от событий. </summary>
    private void OnDisable()
    {
        StationMenu.OnRequireStationBuild -= BuildEquipment;
        Room.OnDeleteEquipmentButtonClick -= DeleteEquipment;
    }

    /// <summary> Инициализация. </summary>
    private void Awake()
    {
        stations = new();
        stationsData = new();
    }
    #endregion

    #region Private Methods
    /// <summary> Постройка оборудования. </summary>
    /// <param name="room"> Комната, в которой строится оборудование. </param>
    /// <param name="data"> Модель оборудования. </param>
    private void BuildEquipment(Room room, StationData data)
    {
        GameObject station = Instantiate(prefabs[(int)data.Type - 1], room.transform, false);
        StationCraft craftScript = station.GetComponentInChildren<StationCraft>();
        RectTransform logicObject = craftScript.GetComponent<RectTransform>();
        StationSelect mainComponent = logicObject.GetComponent<StationSelect>();

        mainComponent.Initialize(data);
        room.Station = mainComponent;

        station.transform.SetAsFirstSibling();
        logicObject.SetParent(stationLogicParent);
        room.StationVisual = station;

        stations.Add(station, craftScript);
        stationsData.Add(station, data);
        StationInventory.Instance.RemoveStation(data);

        OnStationBuilt?.Invoke(craftScript);
    }

    /// <summary> Удаление оборудования из комнаты. </summary>
    /// <param name="room"> Комната. </param>
    private void DeleteEquipment(Room room)
    {
        if (room.StationVisual == null)
            return;
        StationInventory.Instance.AddStation(stationsData[room.StationVisual]);
        room.DestoryEquipment(); //destroying visual object
        Destroy(stations[room.StationVisual].gameObject); //destroying logic object
        OnEquipmentDestroyed?.Invoke();
    }
    #endregion
}
