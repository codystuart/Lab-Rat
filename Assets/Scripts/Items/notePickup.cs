using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notePickup : MonoBehaviour
{
    //notes only
    [SerializeField] noteData note;

    bool canPickup;

    void Update()
    {
        // !! place glow function here !! //

        //if player is near item and presses E, pick it up
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
            inventorySystem.inventory.interact.SetActive(false);
        }
    }

    public void Pickup()
    {
        inventorySystem.inventory.pickupSound.Play();
        inventorySystem.inventory.addNote(note);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
            inventorySystem.inventory.interact.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
            inventorySystem.inventory.interact.SetActive(false);
        }
    }
}