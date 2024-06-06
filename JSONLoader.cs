using UnityEngine;


namespace InhumansUtility
{
    /// <summary> Загрузка JSON ассетов. </summary>
    /// <remarks> Читает JSON файл. </remarks>
    public static class JSONLoader
    {
        public static T LoadFromJson<T>(TextAsset jsonFile)
        {
            return JsonUtility.FromJson<T>(jsonFile.text);
        }
    }
}