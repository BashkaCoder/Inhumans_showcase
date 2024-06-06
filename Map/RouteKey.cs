/// <summary>Класс, отвечающий за ключ маршрута.</summary>
public class RouteKey
{
    /// <summary>Индекс исходящего здания.</summary>
    public int FromBuildingID { get; }
    /// <summary>Индекс входящего здания.</summary>
    public int ToBuildingID { get; }

    /// <summary>Конструктор класса.</summary>
    public RouteKey(int from, int to)
    {
        FromBuildingID = from;
        ToBuildingID = to;
    }

    /// <summary>Получить хешкод для словаря.</summary>
    public override int GetHashCode() => (FromBuildingID, ToBuildingID).GetHashCode();

    /// <summary>Метод расширения, чтобы использовать в качестве ключа в словаре.</summary>
    public override bool Equals(object obj)
    {
        return obj is RouteKey other && FromBuildingID == other.FromBuildingID && ToBuildingID == other.ToBuildingID;
    }
}
