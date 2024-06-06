using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ContainedAspect
{
    public int aspectID;
    public int amount;
}

[Serializable]
public struct ResearchResource
{
    public string name;
    public Sprite imageIcon;
    public List<ContainedAspect> containedAspects;
}

public class ResearchResources : MonoBehaviour
{
    [SerializeField] private List<ResearchResource> researchResources;
}
