using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Aspect
{
    [HideInInspector] public int ID;
    public string name;
    public Sprite imageIcon;
}

public class Aspects : MonoBehaviour
{
    [SerializeField] public List<Aspect> aspects;

    private void Awake()
    {
        for (int i = 0; i < aspects.Count; i++)
        {
            aspects[i].ID = i;
        }
    }
}