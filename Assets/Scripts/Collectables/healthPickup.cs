using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickup : MonoBehaviour, ICollectable, IHealth
{
    [SerializeField] AudioSource pickupSound;
    public void Collect()
    {
        if (gameManager.instance.playerScript.HP < gameManager.instance.playerScript.originalHP)
        {
            //pickupSound.Play();
            gameObject.SetActive(false);
            giveHealth(50);
        }
    }

    public void giveHealth(int amount)
    {
        gameManager.instance.playerScript.HP += amount;

        gameManager.instance.playerScript.updatePlayerUI();
    }
}
