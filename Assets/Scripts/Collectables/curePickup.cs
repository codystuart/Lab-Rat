using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class curePickup : MonoBehaviour, ICollectable
{
    public void Collect()
    {
        gameObject.SetActive(false);
        gameManager.instance.updateCureGameGoal(1);
    }

}
