using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary> Настройки экрана. </summary>
/// <remarks> Класс реализует изменение режима и разрешения экрана. </remarks>
public class ScreenOption : MonoBehaviour
{
    #region Private Fields
    /// <summary> DropDown выбора режима экрана. </summary>
    [SerializeField] private TMP_Dropdown screenModeDropdown;

    /// <summary> DropDown выбора разрешения. </summary>
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    /// <summary> Максимальная частота обновления. </summary>
    private int MaxRefreshRate { get; set; }

    /// <summary> Все уникальные разрешения экрана. </summary>
    private readonly List<Resolution> resolutions = new();

    /// <summary> Опции режима экрана(en). </summary>
    List<string> screenModeOptions = new() {
            "Полный экран",
            "Окно",
        };
    #endregion

    #region Private Methods
    /// <summary> Инициализация настроек при старте. </summary>
    private void Start()
    {
        //Initialize();
    }

    /// <summary> Инициализация системы настроек. </summary>
    /// <summary> Инициализация системы настроек. </summary>
    public void Initialize()
    {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        InitializeResolutions();
        InitializeScreenModes();
        LoadSavedOptions();
        UpdateDropdownValues();
#endif
    }

    /// <summary> Обновить значения Dropdown. </summary>
    private void UpdateDropdownValues()
    {
        resolutionDropdown.value = GetResolutionIndex();
        resolutionDropdown.RefreshShownValue();

        screenModeDropdown.value = GetScreenModeIndex();
        screenModeDropdown.RefreshShownValue();
    }

    /// <summary> Получить индекс текущего разрешения в списке. </summary>
    private int GetResolutionIndex()
    {
        string resolutionWidth = "resolutionWidth", resolutionHeight = "resolutionHeight";
        int currentIndex = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (resolutions[i].width == PlayerPrefs.GetInt(resolutionWidth) &&
                resolutions[i].height == PlayerPrefs.GetInt(resolutionHeight))
            {
                currentIndex = i;
                break;
            }
        }
        return currentIndex;
    }

    /// <summary> Получить индекс текущего режима экрана в списке. </summary>
    private int GetScreenModeIndex()
    {
        string screenMode = "screenMode";
        int screenModeInt = PlayerPrefs.GetInt(screenMode, (int)Screen.fullScreenMode);
        return screenModeInt;
    }

    /// <summary> Инициализация системы разрешений. </summary>
    private void InitializeResolutions()
    {
        var allResolutions = Screen.resolutions;
        MaxRefreshRate = allResolutions[^1].refreshRate;
        var uniqueResolutions = new List<string>();

        foreach (var resolution in allResolutions)
        {
            if (resolution.refreshRate == MaxRefreshRate)
            {
                uniqueResolutions.Add($"{resolution.width}x{resolution.height}");
                resolutions.Add(resolution);
            }
        }

        string resolutionWidth = "resolutionWidth", resolutionHeight = "resolutionHeight";
        int currentIndex = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (resolutions[i].width == PlayerPrefs.GetInt(resolutionWidth) &&
                resolutions[i].height == PlayerPrefs.GetInt(resolutionHeight))
            {
                currentIndex = i;
                break;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(uniqueResolutions);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();
    }

    /// <summary> Инициализация системы режимов экрана. </summary>
    private void InitializeScreenModes()
    {
        screenModeDropdown.ClearOptions();
        List<string> screenModeDropdownLocalizedOptions = screenModeOptions;
        screenModeDropdown.AddOptions(screenModeDropdownLocalizedOptions);
        screenModeDropdown.RefreshShownValue();
    }

    /// <summary> Загрузить сохраненные настройки. </summary>
    private void LoadSavedOptions()
    {
        LoadScreenMode();
        LoadResolution();
    }

    //// <summary> Загрузить сохраненный режим экрана. </summary>
    private void LoadScreenMode()
    {
        string screenMode = "screenMode";
        int screenModeInt = PlayerPrefs.GetInt(screenMode, (int)FullScreenMode.FullScreenWindow);
        screenModeDropdown.value = screenModeInt;
        screenModeDropdown.RefreshShownValue();

        if (screenModeInt == (int)FullScreenMode.FullScreenWindow)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    /// <summary> Загрузить сохраненное разрешение экрана. </summary>
    private void LoadResolution()
    {
        string resolutionWidth = "resolutionWidth", resolutionHeight = "resolutionHeight";
        int width = PlayerPrefs.GetInt(resolutionWidth, resolutions[^1].width);
        int height = PlayerPrefs.GetInt(resolutionHeight, resolutions[^1].height);
        SetResolution(width, height);

    }

    /// <summary> Установить разрешение. </summary>
    /// <param name="width"> Ширина.</param>
    /// <param name="height"> Высота.</param>
    private void SetResolution(int width, int height)
    {
        string resolutionWidth = "resolutionWidth", resolutionHeight = "resolutionHeight";
        Screen.SetResolution(width, height, Screen.fullScreenMode);
        PlayerPrefs.SetInt(resolutionWidth, width);
        PlayerPrefs.SetInt(resolutionHeight, height);
        PlayerPrefs.Save();
    }
    #endregion

    #region Public Methods
    /// <summary> Событие DropDown изменения разрешения экрана. </summary>
    public void OnResolutionOptionChanged()
    {
        Resolution resolution = resolutions[resolutionDropdown.value];
        SetResolution(resolution.width, resolution.height);
    }

    /// <summary> Событие DropDown изменения режима экрана. </summary>
    public void OnScreenModeChanged()
    {
        string screenMode = "screenMode";
        PlayerPrefs.SetInt(screenMode, screenModeDropdown.value);
        PlayerPrefs.Save();
        switch (screenModeDropdown.value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    public void UpdateLocalizedScreenOptions()
    {
        var currentOption = screenModeDropdown.value;

        screenModeDropdown.ClearOptions();
        List<string> screenModeDropdownLocalizedOptions = screenModeOptions;
        screenModeDropdown.AddOptions(screenModeDropdownLocalizedOptions);
        screenModeDropdown.value = currentOption;
        screenModeDropdown.RefreshShownValue();
    }
    #endregion
}