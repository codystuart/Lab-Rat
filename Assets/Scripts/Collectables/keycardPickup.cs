using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycardPickup : MonoBehaviour, ICollectable
{
    [SerializeField] bool keycardAcquired;

    public void Collect()
    {
        Debug.Log("Keycard Acquired");
        gameObject.SetActive(false);
        keycardAcquired = true;
    }


}
