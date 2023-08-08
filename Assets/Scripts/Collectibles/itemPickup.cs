using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    //inventory only - gun and flashlight do not use this
    [SerializeField] itemData data;

    public void Pickup()
    {
        inventorySystem.inventory.Add(data);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //display text "E" when near the object here

            if (Input.GetKeyDown(KeyCode.E))
            {
                Pickup();
            }
        }
    }
}