using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycardPickup : MonoBehaviour, ICollectable
{
    [SerializeField] AudioSource pickupSound;
    public void Collect()
    {
        Debug.Log("Keycard Acquired");
        //pickupSound.Play();
        gameObject.SetActive(false);
        gameManager.instance.keycardAcquired = true;
    }
}
