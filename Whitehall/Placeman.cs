using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> Класс управления чиновником.</summary>
public class Placeman : MonoBehaviour
{
    /// <summary> Спрайт гильдии.</summary>
    [field: SerializeField] public Sprite ClanCoatSprite { get; set; }

    /// <summary> Значение части выполненных контрактов в границах [0, 1].</summary>
    [field: SerializeField] public float TrustNormalized { get; set; }

    /// <summary> Значение выполненных контрактов.</summary>
    private int ContractAccomplished { get; set; }
    /// <summary> Значение общего числа контрактов.</summary>
    private int MaxTrustContractsAmount { get; set; }

    /// <summary> Название сцены.</summary>
    private string sceneName;

    /// <summary> Awake.</summary>
    private void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary> Метод при запуске сцены подсчет части выполненных контрактов.</summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneName)
        {
            SetData(ContractAccomplished, MaxTrustContractsAmount);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary> Подсчет части выполненных контрактов.</summary>
    private void SetData(int contractAccomplished, int maxTrustContractsAmount)
    {
        ContractAccomplished = contractAccomplished;
        MaxTrustContractsAmount = maxTrustContractsAmount;
        TrustNormalized = ContractAccomplished / MaxTrustContractsAmount;
    }
}