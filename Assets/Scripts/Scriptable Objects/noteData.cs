using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class noteData : ScriptableObject
{
    public string title;
    public List<string> noteStrings = new List<string>();
}