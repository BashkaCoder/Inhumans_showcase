using UnityEngine;

/// <summary> Класс, расширяющий функционал AudioSource.</summary>
public static class AudioSourceExtension
{
    /// <summary> Инвертирован ли параметр Pitch.</summary>
    public static bool IsReversePitch(this AudioSource source)
    {
        return source.pitch < 0f;
    }

    /// <summary> Получить оставшееся время трека.</summary>
    public static float GetClipRemainingTime(this AudioSource source)
    {
        float remainingTime = (source.clip.length - source.time) / source.pitch;
        return source.IsReversePitch() ?
               (source.clip.length + remainingTime) : remainingTime;
    }
}
