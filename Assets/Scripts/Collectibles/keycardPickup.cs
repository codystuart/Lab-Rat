using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class keycardPickup : MonoBehaviour, ICollectible
{

    public void Collect()
    {
        Debug.Log("Keycard Acquired");
        gameObject.SetActive(false);
        gameManager.instance.keycardAcquired = true;
    }


}
