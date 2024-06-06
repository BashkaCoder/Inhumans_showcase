using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>Класс, отвечающий за управление путями карты.</summary>
public class RouteController : MonoBehaviour
{
    /// <summary> Контейнер хранения маршрутов. </summary>
    [SerializeField] private RectTransform RoutesContainer;
    /// <summary>  Словарь всех доступных маршрутов передвижения по карте.</summary>
    private readonly Dictionary<RouteKey, RectTransform[]> routes = new();

    /// <summary>Запуск скрипта.</summary>
    private void Awake()
    {
        InitialiseRoutes();
    }

    /// <summary>Добавить маршрут.</summary>
    public void AddRoute(int fromBuildingID, int toBuildingID, RectTransform[] waypoints)
    {
        var key = new RouteKey(fromBuildingID, toBuildingID);
        routes[key] = waypoints;
    }

    /// <summary>Получить маршрут.</summary>
    public RectTransform[] GetRoute(int fromBuildingID, int toBuildingID)
    {
        var key = new RouteKey(fromBuildingID, toBuildingID);

        if (!routes.ContainsKey(key))
            Debug.LogError($"Route: from({fromBuildingID}) to ({toBuildingID}) doesn't exist.");

        return routes[key];
    }

    /// <summary>Инициализировать маршруты.</summary>
    private void InitialiseRoutes()
    {
        ParseAllRoutes();
    }

    /// <summary>Спарсить все маршруты.</summary>
    private void ParseAllRoutes() => RoutesContainer.Cast<RectTransform>().ToList().ForEach(ParseRouteObject);

    /// <summary>Спарсить маршрут.</summary>
    private void ParseRouteObject(RectTransform routeObject)
    {
        if (routeObject == null)
        {
            Debug.LogError("Route object not found!");
            return;
        }

        // Извлекаем инты из имени объекта
        int[] routeIndices = ExtractIndicesFromName(routeObject.name);

        if (routeIndices.Length != 2)
        {
            Debug.LogError("Invalid route object name format!");
            return;
        }

        int index1 = routeIndices[0];
        int index2 = routeIndices[1];

        // Получаем дочерние объекты
        RectTransform[] waypoints = new RectTransform[routeObject.childCount];
        for (int i = 0; i < routeObject.childCount; i++)
        {
            waypoints[i] = routeObject.GetChild(i).GetComponent<RectTransform>();
        }

        AddRoute(index1, index2, waypoints);
        AddRoute(index2, index1, waypoints.Reverse().ToArray());
    }

    /// <summary>Извлечь индексы из имени маршрута.</summary>
    private int[] ExtractIndicesFromName(string name)
    {
        // Заменяем все символы, кроме цифр и знака подчеркивания, на пустую строку
        string cleanedName = new(name.Where(c => char.IsDigit(c) || c == '_').ToArray());

        string[] indices = cleanedName.Split('_');

        if (indices.Length != 2)
        {
            Debug.LogError($"Wrong RouteNameFormat: {name}!");
            return new int[0];
        }

        if (int.TryParse(indices[0], out int index1) &&
            int.TryParse(indices[1], out int index2))
        {
            return new int[] { index1, index2 };
        }

        Debug.LogError($"Couldn't extract IDs from route: {name}");
        return null;
    }
}
