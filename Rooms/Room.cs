using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>Enum типов комнат.</summary>
[Serializable]
public enum RoomTypeEnum
{
    NullType,
    FirstType,
    SecondType,
    ThirdType
}

/// <summary>Класс комнаты.</summary>
public class Room : MonoBehaviour
{
    /// <summary>Событие удаления оборудования из комнаты.</summary>
    public static event Action<Room> OnDeleteEquipmentButtonClick;
    /// <summary>Обработчик комнаты.</summary>
    public RoomHandler RoomHandler { get; private set; }

    /// <summary>Ссылка на GridManager.</summary>
    public GridManager GridManager { get; private set; }

    /// <summary>Массив изображений для комнат.</summary>
    public Sprite RoomImage { get; private set; }
    /// <summary>Массив  спрайтов алхимии.</summary>
    public Sprite[] RoomImagesAlc { get; private set; }
    /// <summary>Массив спрайтов кузнечества.</summary>
    public Sprite[] RoomImagesSmith { get; private set; }
    /// <summary>Массив спарйтов столяров.</summary>
    public Sprite[] RoomImagesJoiner { get; private set; }

    /// <summary>Кнопка удаления оборудования.</summary>
    [SerializeField] private Button DelEquipmentButton;

    /// <summary>Панель выбора оборудования.</summary>
    public GameObject EquipmentPanelButton;

    /// <summary>Изображение комнаты.</summary>
    public Image Image { get; private set; }

    /// <summary>Кнопка добавления оборудования.</summary>
    public Button AddEquipmentButton;

    /// <summary>Поле: Тип комнаты.</summary>
    private RoomType _roomTypeState;
    /// <summary>Свойство: Тип комнаты.</summary>
    public RoomType RoomTypeState
    {
        get => _roomTypeState;
        set
        {
            // Доп. логика, если нада
            _roomTypeState = value;
        }
    }

    /// <summary>Конструктор комнаты.</summary>
    /// <param name="rt">Стате комнаты.</param>
    public Room(RoomType rt) => RoomTypeState = rt;

    /// <summary>Стате комнаты из Enum.</summary>
    private RoomTypeEnum roomTypeEnum;
    /// <summary>Публичное поле типа комнаты.</summary>
    public RoomTypeEnum RoomTypeEnum { get => roomTypeEnum; set => roomTypeEnum = value; }

    /// <summary>Длина комнаты.</summary>
    public int RoomLength { get; set; }

    /// <summary>Вектор с позицией комнаты на канвасе.</summary>
    public Vector2 position;
    /// <summary> Визуал оборудования.</summary>
    public GameObject StationVisual;

    private StationSelect station;
    public StationSelect Station { get; set; }
    

    /// <summary>Инициализация переменных.</summary>
    private void Awake()
    {
        RoomTypeState = new NullType();
        GridManager = GridManager.Instance;
        RoomImage = LoadSprite("rooms/1to1");
        RoomImagesAlc = LoadSpriteSheet("rooms/alc");
        RoomImagesJoiner = LoadSpriteSheet("rooms/joiner");
        RoomImagesSmith = LoadSpriteSheet("rooms/smith");

        Image = GetComponent<Image>();
        RoomHandler = new(this);
    }
    /// <summary>Загрузка спрайтов из Resources.</summary>
    /// <param name="path">Путь загрузки.</param>
    /// <returns>Спрайт.</returns>
    private Sprite LoadSprite(string path)
    {
        Sprite sprite = Resources.Load<Sprite>(path);
        return sprite;
    }
    /// <summary>Загрузка массива спрайтов комнат в массив.</summary>
    /// <returns>Массив спрайтов.</returns>
    private Sprite[] LoadSpriteSheet(string path)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);
        return sprites;
    }

    /// <summary>Замена изображения.</summary>
    /// <param name="roomImage">индекс изображения.</param>
    public void ChangeImage(int roomImage, RoomTypeEnum type)
    {
        RoomTypeState.ChangeSprite(Image, roomImage);
        //Image.sprite = RoomImage[roomImage];
    }

    /// <summary>Активация панельки выбора типа оборудования.</summary>
    public void OnAddEquipmentButtonClick()
    {
        RoomHandler.OnAddEquipmentButtonClick();
    }

    /// <summary>Изменение типа комнаты.</summary>
    /// <param name="typeButton">Тип комнаты в стринге, передаваемый через кнопку.</param>
    public void OnEquipmentButtonClick(Room r, StationData data)
    {
        if (r == this)
            RoomHandler.OnEquipmentButtonClick(data.Type);
    }

    /// <summary>Удаление оборудования из комнаты по кнопке.</summary>
    public void HandleDeleteButtonClick()
    {
        RoomHandler.OnDeleteEquipmentButtonClick();
        OnDeleteEquipmentButtonClick?.Invoke(this);
    }

    /// <summary>Включение режима строительства по кнопке BuildButton.</summary>
    public void SetEquipmentOn(bool setting)
    {
        if (setting)
        {
            if (RoomTypeState is NullType)
                AddEquipmentButton.gameObject.SetActive(true);
        }
        else
        {
            if (RoomTypeState is not NullType)
            {
                DelEquipmentButton.gameObject.SetActive(true);
            }
        }
    }
    /// <summary>Выключение спрайтов строительства оборудования.</summary>
    public void SetEquipmentOff()
    {
        AddEquipmentButton.gameObject.SetActive(false);
        DelEquipmentButton.gameObject.SetActive(false);
        EquipmentPanelButton.SetActive(false);
    }

    /// <summary>Принудительное выключение режима строительства по кнопке BuildButton.</summary>
    public void DisableBuildingState()
    {
        AddEquipmentButton.gameObject.SetActive(false);
        DelEquipmentButton.gameObject.SetActive(false);
        EquipmentPanelButton.SetActive(false);
    }

    /// <summary>Переключение состояния активности кнопки добавления оборудования.</summary>
    /// <param name="state">Булевое состояние.</param>
    public void SetAddEquipmentButtonActivity(bool state)
    {
        AddEquipmentButton.gameObject.SetActive(state);
    }

    /// <summary>Переключение состояния активности панели оборудования.</summary>
    /// <param name="state">Булевое состояние.</param>
    public void SetEquipmentPanelActivity(bool state)
    {
        EquipmentPanelButton.SetActive(state);
    }

    /// <summary>Удаление оборудования.</summary>
    public void DestoryEquipment()
    {
        Station.UnlinkItem();
        Station.UnlinkWorker();
        Destroy(StationVisual);
    }
}
