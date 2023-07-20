using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class saveStats : ScriptableObject
{
    [Header("----- Components -----")]
    public List<gunStats> gunListSave = new List<gunStats>();
}