using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary> Класс потокобезопасного генератора случайных чисел.</summary>
public static class ThreadSafeRandom
{
    [ThreadStatic] private static System.Random Local;

    public static System.Random ThisThreadsRandom
    {
        get { return Local ?? (Local = new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
    }
}

/// <summary> Класс расширений работы со списками.</summary>
static class Extensions
{
    /// <summary> Перемешать элементы списка.</summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    /// <summary> Получить случайный элемент.</summary>
    public static T GetRandomElement<T>(T[] elements)
    {
        if (elements.Length == 0)
        {
            Debug.LogWarning("Provided empty collection");
            return default(T);
        }
        var random = new System.Random();
        int rindex = random.Next(elements.Length);
        return elements[rindex];
    }
}
