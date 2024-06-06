using UnityEngine;

/// <summary>Класс, отвечающий за...</summary>
public class FormulaUtils : MonoBehaviour
{
    [Header("Time Coeffs")]
    [SerializeField] private float timePerStationLvl;
    [SerializeField] private float timePerEmpLvl;

    [Header("Bonus Prod Coeffs")]
    [SerializeField] private float bonusPerStationLvl;
    [SerializeField] private float bonusPerEmpLvl;

    [Header("Quality Coeffs")]
    [SerializeField] private float qualityPerStationLvl;
    [SerializeField] private float qualityPerEmpLvl;

    [Header("Resource Saving Coeffs")]
    [SerializeField] private float savingPerStationLvl;
    [SerializeField] private float savingPerEmpLvl;

    [Header("Ref")]
    [SerializeField] private CharacterUpgradeCalculator charCalculator;
    [SerializeField] private UpgradeController upgradeController;

    public static FormulaUtils Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    public float StationPercents(StationUpgradeEnum statEnum, int stationLvl)
    {
        switch (statEnum)
        {
            case StationUpgradeEnum.ResourceEconomy:
                return (stationLvl * savingPerStationLvl);
            case StationUpgradeEnum.DoubleProductChance:
                return (stationLvl * bonusPerStationLvl);
            case StationUpgradeEnum.CraftingSpeed:
                return (stationLvl * timePerStationLvl);
            case StationUpgradeEnum.CraftingAmount:
                return (stationLvl * qualityPerEmpLvl);
            default:
                return 0;
        }
    }

    public float TimeScale(int stationLvl, int empLvl, int empAttitude)
    {
        float time = 1;
        float charPercent = charCalculator.CalculateUpgradeValue(CharacterUpgrades.WorkingSpeed,
            upgradeController.GetCharStatLevel(CharacterUpgrades.WorkingSpeed));

        float stationPercent = stationLvl * timePerStationLvl;
        float empPercent = empLvl * timePerEmpLvl;
        float buffPercent = charPercent + stationPercent + empPercent + (empAttitude >= 0 ? (empAttitude * 2) : (empAttitude)); //10
        time += time / 100 * buffPercent; //1.1
        return time;
    }

    public float ProductionVolume(int resValue, int stationLvl, int empLvl)
    {
        float stationPercent = resValue / 100 * stationLvl * bonusPerStationLvl;
        float empPercent = resValue / 100 * empLvl * bonusPerEmpLvl;
        return resValue * (1 + stationPercent + empPercent);
    }

    public float QualityChance(int stationLvl, int empLvl) 
    {
        float stationPercent = time / 100 * stationLvl * qualityPerStationLvl;
        float empPercent = time / 100 * empLvl * qualityPerEmpLvl;
        return stationPercent + empPercent;
    }

    public int ResourceSaving(int resource, int stationLvl, int empLvl)
    {
        float charPercent = charCalculator.CalculateUpgradeValue(CharacterUpgrades.FreeCraft,
            upgradeController.GetCharStatLevel(CharacterUpgrades.WorkingSpeed));

        float stationPercent = resource / 100 * stationLvl * savingPerStationLvl;
        float empPercent = resource / 100 * empLvl * savingPerEmpLvl;
        return (int)(resource * (1 - charPercent - stationPercent - empPercent));
    }
}
