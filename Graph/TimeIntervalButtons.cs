using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary> Переключение режимов отображения. </summary>
/// <remarks> Меню выбора временного интервала. </remarks>
public class TimeIntervalButtons : MonoBehaviour
{
    /// <summary> Событие при нажатии кнопки выбора. </summary>
    public event Action<TimeInterval> OnTimeButtonPressed;

    /// <summary> Кнопки выбора. </summary>
    [SerializeField] private List<Button> buttons;

    #region MonoBehaviour Methods
    /// <summary> Назначение функционала кнопкам при запуске сцены. </summary>
    private void Start()
    {
        AssignButtons();
    }
    #endregion

    #region Private Methods
    /// <summary> Назначение функционала кнопкам. </summary>
    private void AssignButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];
            int index = i;
            button.onClick.AddListener(delegate
            {
                HandleButtonPress(index);
            });
        }
    }

    /// <summary> Обработка нажатий на кнопки. </summary>
    /// <param name="i"> Индекс кнопки. </param>
    private void HandleButtonPress(int i)
    {
        OnTimeButtonPressed?.Invoke((TimeInterval)i);
    }
    #endregion
}
