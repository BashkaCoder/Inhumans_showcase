using UnityEngine;
using UnityEngine.UI;

/// <summary> Наустройки аудио.</summary>
public class AudioOption : MonoBehaviour
{
    /// <summary> Зарезервированное название сохранения общей громкости.</summary>
    private const string MASTER_PREF = "masterPref";
    /// <summary> Зарезервированное название сохранения громкости звуков.</summary>
    private const string SFX_PREF = "sfxPref";
    /// <summary> Зарезервированное название сохранения громкости ambient-треков.</summary>
    private const string AMBIENT_PREF = "ambientPref";

    [Header("Sliders")]
    /// <summary> Слайдер изменения общей громкости.</summary>
    [SerializeField] private Slider masterSlider;
    /// <summary> Слайдер изменения громкости звуков.</summary>
    [SerializeField] private Slider sfxSlider;
    /// <summary> Слайдер изменения громкости треков.</summary>
    [SerializeField] private Slider ambientMusicSlider;

    /// <summary> Загрузить сохраненные настройки.</summary>
    private void LoadDefaultOrSavedSoundSettings()
    {
        //Don't ask! Разрабы Дауны
        masterSlider.value = PlayerPrefs.GetFloat(MASTER_PREF, 1.0f) + 0.001f;
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_PREF, 1.0f) + 0.001f;
        ambientMusicSlider.value = PlayerPrefs.GetFloat(AMBIENT_PREF, 1.0f) + 0.001f;
    }

    /// <summary> Сохранение настроек при потере фокуса приложением.</summary>
    private void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
            SaveSoundSettings(); // при закрытии или сворачивании настройки сохраняются
    }

    /// <summary> Инициализация настроек аудио.</summary>
    public void Initialize()
    {
        AssignSlidersListeners();
        LoadDefaultOrSavedSoundSettings();
    }

    /// <summary> Добавить слайдеры ползункам.</summary>
    private void AssignSlidersListeners()
    {
        masterSlider.onValueChanged.AddListener((System.Single masterVolume) => AudioManager.Instance.OnMasterVolumeChanged(masterVolume));
        sfxSlider.onValueChanged.AddListener((System.Single sfxVolume) => AudioManager.Instance.OnSFXVolumeChanged(sfxVolume));
        ambientMusicSlider.onValueChanged.AddListener((System.Single ambientMusicVolume) => AudioManager.Instance.OnMusicVolumeChanged(ambientMusicVolume));
    }

    /// <summary> Сохранить настройки аудио.</summary>
    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(MASTER_PREF, masterSlider.value);
        PlayerPrefs.SetFloat(SFX_PREF, sfxSlider.value);
        PlayerPrefs.SetFloat(AMBIENT_PREF, ambientMusicSlider.value);
        PlayerPrefs.Save();
    }

    /// <summary> Сохранение настроек при выходе из приложения.</summary>
    private void OnApplicationQuit()
    {
        SaveSoundSettings();
    }
}