using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class itemData : ScriptableObject
{
    //item data
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
}