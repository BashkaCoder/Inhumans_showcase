using UnityEngine;

[CreateAssetMenu(fileName = "NewResearchNode", menuName = "GameCore/Research/Research Node")]
public class ResearchNode : ScriptableObject
{
    public ResearchNode[] parents; // Массив родительских узлов
    public string researchName;
    public string description;
    public int cost;
    public ResearchNode[] children;
    public int depth;

    public bool isLocked = true;
    public Sprite image;
}