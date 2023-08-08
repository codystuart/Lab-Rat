using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    //inventory only (health, cure, ammo, batteries, keycard)
    [SerializeField] itemData data;

    bool canPickup;

    void Update()
    {
        if (canPickup)
        {
            inventorySystem.inventory.interact.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                inventorySystem.inventory.interact.SetActive(false);
                Pickup();
            }
        }
    }

    public void Pickup()
    {
        inventorySystem.inventory.Add(data);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            canPickup = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canPickup = false;
    }
}