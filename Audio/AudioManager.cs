using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static AmbientLocationEnum;

[RequireComponent(typeof(AudioSource))]
/// <summary> Класс, отвечающий за проигрывание треков во время геймплея.</summary>
public class AudioManager : MonoBehaviour
{
    public const string MASTER_VOLUME = "MasterVolume";
    public const string MUSIC_VOLUME = "MusicVolume";
    public const string SFX_VOLUME = "SFXVolume";

    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;

    public LocationType CurrentLocation { get; set; }

    [Header("Location tracks")]
    public List<AudioClip> FactoryTracks;
    public List<AudioClip> BankTracks;
    public List<AudioClip> WhitehallTracks;
    public List<AudioClip> AlchemyGuildTracks;
    public List<AudioClip> JoineryGuildTracks;
    public List<AudioClip> SmitheryGuildTracks;
    public List<AudioClip> MenuTracks;

    /// <summary> Словарь локаций ко всем трекам локации.</summary>
    public Dictionary<LocationType, List<AudioClip>> LocationTracks;
    /// <summary> Словарь локаций к индексу трека.</summary>
    public Dictionary<LocationType, int> LocationToTrackIndex;
    /// <summary> Словарь локаций к временной метке.</summary>
    private Dictionary<LocationType, float> LocationToTrackTime;

    /// <summary> Спиоск треков текущей локации.</summary>
    private List<AudioClip> trackList;
    /// <summary> Индекс текущего трека.</summary>
    private int currentTrackIndex;

    /// <summary> Длительность смешивания треков.</summary>
    public float fadeDuration; // Длительность фейда

    /// <summary> Первый источник аудио.</summary>
    private AudioSource audioPlayer;
    /// <summary> Второй источник аудио.</summary>
    private AudioSource audioPlayerFade; // Новый AudioSource для фейда

    /// <summary> Инициализация и создание компонентов.</summary>
    private void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
        audioPlayerFade = gameObject.AddComponent<AudioSource>(); // Добавляем новый AudioSource для фейда
        audioPlayerFade.outputAudioMixerGroup = audioPlayer.outputAudioMixerGroup;

        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    /// <summary> Запуск конфигурации скрипта аудио-менеджера.</summary>
    private void Start()
    {
        Initialize();
        CurrentLocation = LocationType.Menu;
        trackList = LocationTracks[CurrentLocation];
        PlayNextTrackWithFade(0f); // Начинаем воспроизведение первого трека без фейда
    }

    /// <summary> кортина проигрывания следующего трека.</summary>
    private void PlayNextTrackWithFade(float startFadeTime)
    {
        StartCoroutine(FadeAndPlayTrack(startFadeTime));
    }

    /// <summary> Корутина плавного смешения треков.</summary>
    private IEnumerator FadeAndPlayTrack(float startFadeTime)
    {
        // Ждем, пока не начнется фейд
        yield return new WaitForSeconds(startFadeTime);

        // Сохраняем текущий клип и время
        AudioClip currentClip = audioPlayer.clip;
        float currentTime = audioPlayer.time;

        // Настраиваем AudioSource для фейда
        audioPlayerFade.clip = trackList[currentTrackIndex];
        audioPlayerFade.time = LocationToTrackTime[CurrentLocation];
        audioPlayerFade.volume = 0f;
        audioPlayerFade.Play();

        // Фейд-аут текущего трека и фейд-ин нового трека
        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            audioPlayer.volume = Mathf.Lerp(1f, 0f, t / fadeDuration);
            audioPlayerFade.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        // Останавливаем текущий трек и обновляем AudioSource
        audioPlayer.Stop();
        audioPlayer.clip = currentClip;
        audioPlayer.time = currentTime;

        // Обновляем AudioSource и начинаем воспроизведение следующего трека
        audioPlayer.clip = audioPlayerFade.clip;
        audioPlayer.time = audioPlayerFade.time;
        audioPlayer.volume = 1f;
        audioPlayer.Play();
        audioPlayerFade.Stop();

        StartCoroutine(PlayNextTrackAfterDelay(audioPlayer.GetClipRemainingTime()));
        LocationToTrackIndex[CurrentLocation] = currentTrackIndex + 1;
    }

    /// <summary> Проигрывание следующего трека без задержки.</summary>
    private void PlayNextTrack()
    {
        audioPlayer.clip = trackList[currentTrackIndex];
        audioPlayer.Play();
        StartCoroutine(PlayNextTrackAfterDelay(audioPlayer.clip.length));
        LocationToTrackIndex[CurrentLocation] = currentTrackIndex + 1;
    }

    /// <summary> Корутина проигрывания следюущего стрека после задержки.</summary>
    private IEnumerator PlayNextTrackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (currentTrackIndex >= trackList.Count - 1)
        {
            trackList.Shuffle();
            currentTrackIndex = 0;
        }
        else
        {
            currentTrackIndex++;
        }
        PlayNextTrack();
    }

    /// <summary> Инициализация аудио-менеджера.</summary>
    private void Initialize()
    {
        LocationTracks = new() {
            {LocationType.Factory,       FactoryTracks},
            {LocationType.Bank,          BankTracks},
            {LocationType.Whitehall,     WhitehallTracks},
            {LocationType.AlchemyGuild,  AlchemyGuildTracks},
            {LocationType.JoineryGuild,  JoineryGuildTracks},
            {LocationType.SmitheryGuild, SmitheryGuildTracks},
            {LocationType.Menu,          MenuTracks}
        };
        LocationToTrackIndex = new() {
            {LocationType.Factory,       0},
            {LocationType.Bank,          0},
            {LocationType.Whitehall,     0},
            {LocationType.AlchemyGuild,  0},
            {LocationType.JoineryGuild,  0},
            {LocationType.SmitheryGuild, 0},
            {LocationType.Menu,          0}
        };
        LocationToTrackTime = new() {
            {LocationType.Factory,       0},
            {LocationType.Bank,          0},
            {LocationType.Whitehall,     0},
            {LocationType.AlchemyGuild,  0},
            {LocationType.JoineryGuild,  0},
            {LocationType.SmitheryGuild, 0},
            {LocationType.Menu,          0}
        };

        ShuffleAllAmbientTracks();

        currentTrackIndex = 0;
        audioPlayer.pitch = 1;
    }

    /// <summary> Перетасовка всех локационных треков.</summary>
    private void ShuffleAllAmbientTracks()
    {
        foreach (var pair in LocationTracks)
        {
            pair.Value.Shuffle();
        }
    }

    /// <summary> Покидание локации.</summary>
    public void LeaveLocation()
    {
    }

    /// <summary> Переход в новую локацию.</summary>
    public void EnterLocation(LocationType newLocation)
    {
        if (newLocation == CurrentLocation) return;

        StopAllCoroutines();
        if (audioPlayer.isPlaying)
        {
            LocationToTrackTime[CurrentLocation] = audioPlayer.time;
        }

        CurrentLocation = newLocation;
        trackList = LocationTracks[CurrentLocation];
        currentTrackIndex = Mathf.Clamp(LocationToTrackIndex[CurrentLocation] - 1, 0, trackList.Count - 1);

        if (LocationToTrackTime[CurrentLocation] > 0)
        {
            PlayNextTrackWithFade(fadeDuration); // Начинаем воспроизведение нового трека с фейдом
        }
        else
        {
            PlayNextTrackWithFade(0f); // Начинаем воспроизведение нового трека без фейда
        }
    }

    /// <summary> Событие изменения общей громкости.</summary>
    public void OnMasterVolumeChanged(System.Single masterLevel)
    {
        audioMixer.SetFloat(MASTER_VOLUME, Mathf.Log10(masterLevel) * 20);
    }

    /// <summary> Событие изменения громкости музыки.</summary>
    public void OnMusicVolumeChanged(System.Single musicLevel)
    {
        audioMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(musicLevel) * 20);
    }

    /// <summary> Событие изменения громкости SFX.</summary>
    public void OnSFXVolumeChanged(System.Single sfxLevel)
    {
        audioMixer.SetFloat(SFX_VOLUME, Mathf.Log10(sfxLevel) * 20);
    }
}

/// <summary> Перечисления возможных локаций.</summary>
public static class AmbientLocationEnum
{
    public enum LocationType
    {
        Factory, Bank, Whitehall, AlchemyGuild, JoineryGuild, SmitheryGuild, Menu
    }
}
