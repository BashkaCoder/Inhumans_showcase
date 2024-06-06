using System;

/// <summary>Класс управления специализацией комнат.</summary>
public class RoomHandler
{
    /// <summary>Событие выбранной комнаты.</summary>
    public static event Action<Room> OnRoomSelected;
    /// <summary>Комната. </summary>
    private readonly Room room;
    /// <summary>Конструктор RoomHandler.</summary>
    /// <param name="room">Комната.</param>
    public RoomHandler(Room room)
    {
        this.room = room;
    }

    /// <summary>Метод активации выбора специализации (оборудования).</summary>
    public void OnAddEquipmentButtonClick()
    {
        OnRoomSelected?.Invoke(room);
    }

    /// <summary>Метод удаления специализации комнаты.</summary>
    public void OnDeleteEquipmentButtonClick()
    {
        RoomTypeEnum temp = room.RoomTypeEnum;
        room.RoomTypeState.ChangeTypeNull(room);
        room.RoomTypeState.Check();
        room.RoomTypeState.ChangeSprite(room.Image, 0);
        room.RoomLength = 1;
        room.GridManager.UpdateRooms(temp, room.position, true);
    }

    /// <summary>Метод добавления специализации комнаты.</summary>
    public void OnEquipmentButtonClick(RoomTypeEnum type)
    {
        room.RoomTypeState.ChangeTypeFromNull(room, type);
        room.RoomTypeState.Check();
        room.EquipmentPanelButton.SetActive(false);
        room.AddEquipmentButton.gameObject.SetActive(false);
        room.GridManager.UpdateRooms(type, room.position, false);
    }
}
