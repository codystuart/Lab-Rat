using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    [Header("----- Components -----")]
    public GameObject gunModel;
    public GameObject[] gunChildren;
    public ParticleSystem hitEffect;

    [Header("----- Stats -----")]
    public float shootRate;
    public int shootDamage;
    public int shootDist;
    public int currAmmo;
    public int maxAmmo;
}