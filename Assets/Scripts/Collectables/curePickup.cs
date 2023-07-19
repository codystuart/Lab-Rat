using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class curePickup : MonoBehaviour, ICollectable
{
    [SerializeField] AudioSource pickupSound;
    public void Collect()
    {
        //pickupSound.Play();
        gameObject.SetActive(false);
        gameManager.instance.updateCureGameGoal(1);
    }

}
