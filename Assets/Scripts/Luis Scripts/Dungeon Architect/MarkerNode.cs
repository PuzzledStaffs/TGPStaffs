using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerNode : ScriptableObject
{

    public string MarkerName;

    public MarkerNode(string name)
    {
        MarkerName = name;
    }

    public GameObject Item;
}
