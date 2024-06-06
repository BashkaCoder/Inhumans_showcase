using UnityEngine;

/// <summary>Класс, отвечающий за инициализацию настроек.</summary>
public class OptionsInitializer : MonoBehaviour
{
    [SerializeField] private AudioOption audioOption;
    [SerializeField] private ScreenOption screenOption;

    /// <summary> Запуск инициализации.</summary>
    private void Start()
    {
        audioOption.Initialize();
        screenOption.Initialize();
    }
}
