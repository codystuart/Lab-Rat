using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu]

public class saveStats : ScriptableObject
{
    [Header("----- Lists to Save -----")]
    public List<gunStats> saveGunList = new List<gunStats>();
    public List<itemData> saveInvItems = new List<itemData>();
    public List<noteData> saveNotes = new List<noteData>();

    [Header("----- Flashlight Save -----")]
    public bool saveFlashlight;
    public float fLightIntensity;
    public GameObject saveFlashlightPrefab;
}