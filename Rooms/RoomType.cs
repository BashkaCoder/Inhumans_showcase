using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Абстрактный класс для паттерна состояния для комнат.
/// </summary>
public abstract class RoomType
{
    /// <summary>Проверить комнату.</summary>
    public abstract void Check();
    /// <summary>Изменить тип комнаты.</summary>
    public virtual void ChangeTypeNull(Room room)
    {
        room.RoomTypeEnum = RoomTypeEnum.NullType;
        room.RoomTypeState = new NullType();
    }

    /// <summary>Изменить тип комнаты с никакого.</summary>
    public virtual void ChangeTypeFromNull(Room room, RoomTypeEnum type) { }
    /// <summary>Изменить изображение комнаты.</summary>
    public abstract void ChangeSprite(Image image, int roomIndex);

}

/// <summary>Паттерн фабрика для создания комнат.</summary>
public class RoomTypeFactory
{
    /// <summary>Словарь комнат.</summary>
    private static readonly Dictionary<RoomTypeEnum, Type> RoomTypeDictionary = new()
    {
        { RoomTypeEnum.FirstType, typeof(FirstType) },
        { RoomTypeEnum.SecondType, typeof(SecondType) },
        { RoomTypeEnum.ThirdType, typeof(ThirdType) }
    };

    /// <summary>Создать комнату определенной специализации.</summary>
    public static SpecialRoomType CreateRoomType(RoomTypeEnum type)
    {
        if (RoomTypeDictionary.TryGetValue(type, out Type roomType))
        {
            return (SpecialRoomType)Activator.CreateInstance(roomType);
        }
        return null;
    }
}

/// <summary>
/// Пустой тип комнаты.
/// </summary>
public class NullType : RoomType
{
    /// <summary>Проверить комнату.</summary>
    public override void Check()
    {
        Debug.Log("Hi, I'm null type of room");
    }
    /// <summary>Изменить тип комнаты с никакого.</summary>
    public override void ChangeTypeFromNull(Room room, RoomTypeEnum type)
    {
        room.RoomTypeEnum = type;
        room.RoomTypeState = RoomTypeFactory.CreateRoomType(type);
    }
    /// <summary>Изменить изображение комнаты.</summary>
    public override void ChangeSprite(Image image, int index)
    {
        image.sprite = Resources.Load<Sprite>("rooms/1to1");
    }
}

/// <summary>
/// Базовый класс для типов комнат с цветом.
/// </summary>
public abstract class SpecialRoomType : RoomType
{
    /// <summary>Получить изображение комнаты.</summary>
    protected abstract Sprite[] GetRoomSprite();
    /// <summary>Изменить изображение комнаты.</summary>
    public override void ChangeSprite(Image image, int index)
    {
        image.sprite = GetRoomSprite()[index];
    }
}

/// <summary>
/// Первый тип комнаты.
/// </summary>
class FirstType : SpecialRoomType
{
    /// <summary>Проверить комнату.</summary>
    public override void Check()
    {
        //Debug.Log("Hi, I'm first type of room");
    }
    /// <summary>Получить изображение комнаты.</summary>
    protected override Sprite[] GetRoomSprite()
    {
        return Resources.LoadAll<Sprite>("rooms/alc");
    }
}

/// <summary>
/// Второй тип комнаты.
/// </summary>
class SecondType : SpecialRoomType
{
    /// <summary>Проверить комнату.</summary>
    public override void Check()
    {
        //Debug.Log("Hi, I'm second type of room");
    }
    /// <summary>Получить изображение комнаты.</summary>
    protected override Sprite[] GetRoomSprite()
    {
        return Resources.LoadAll<Sprite>("rooms/smith");
    }
}

/// <summary>
/// Третий тип комнаты.
/// </summary>
class ThirdType : SpecialRoomType
{
    /// <summary>Проверить комнату.</summary>
    public override void Check()
    {
        //Debug.Log("Hi, I'm third type of room");
    }
    /// <summary>Получить изображение комнаты.</summary>
    protected override Sprite[] GetRoomSprite()
    {
        return Resources.LoadAll<Sprite>("rooms/joinery");
    }
}