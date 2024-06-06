using System;
using System.Collections;
using UnityEngine;

/// <summary>Класс, отвечающий за передвижение игрока по карте.</summary>
public class PlayerMovement : MonoBehaviour
{
    // Action прибытия игрока
    /// <summary> Событие прибытия игрока.</summary>
    public event Action OnPlayerArrived;

    /// <summary>Список точек пути.</summary>
    public RectTransform[] RouteWaypoints { get; set; } // Массив целевых позиций
    /// <summary>Флаг передвижения игрка.</summary>
    public bool IsMoving { get; private set; }
    /// <summary>Скорость передвижения игрока.</summary>
    [SerializeField] private float moveSpeed; // Скорость движения игрока

    /// <summary>Индекс локации, к которой направляемся.</summary>
    private int currentTargetIndex = 0;

    /// <summary>Передвинуться к следующей цели.</summary>
    public void MoveToNextTarget()
    {
        IsMoving = true;
        if (currentTargetIndex < RouteWaypoints.Length)
        {
            StartCoroutine(MovePlayer(RouteWaypoints[currentTargetIndex].position));
        }
        else
        {
            IsMoving = false;
            currentTargetIndex = 0;
        }
    }

    /// <summary>Корутина передвижения игрока.</summary>
    IEnumerator MovePlayer(Vector3 targetPosition)
    {
        Vector3 startingPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        transform.position = targetPosition;

        if (currentTargetIndex == RouteWaypoints.Length - 1)
            OnPlayerArrived?.Invoke();

        currentTargetIndex++;

        MoveToNextTarget();
    }
}