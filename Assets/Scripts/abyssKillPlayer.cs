using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abyssKillPlayer : MonoBehaviour
{
    [SerializeField] int damage;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.TakeDamage(damage);
        }
    }

}