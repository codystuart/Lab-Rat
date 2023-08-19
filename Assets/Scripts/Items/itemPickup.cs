using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    //inventory only (health, cure, ammo, batteries, keycard)
    [SerializeField] itemData item;

    bool canPickup;

    void Update()
    {
        //!! place glow function here after completed  (maybe remove rotation) !!//

        //if player is near item and presses E, pick it up
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
            inventorySystem.inventory.interact.SetActive(false);
        }
    }

    public void Pickup()
    {
        //while the amount of held items is less than the max
        //add them to the inventory
        if (inventorySystem.inventory.items.Count > inventorySystem.inventory.maxItems - 1)
            StartCoroutine(inventoryFull());
        else
        {
            if (item.id == 'c')
                gameManager.instance.updateCureGameGoal(1);
            if (item.id == 'k')
                gameManager.instance.keycardAcquired = true;

            inventorySystem.inventory.addItem(item);
            gameManager.instance.save.saveInvItems.Add(item); //save the items to the save list
            Destroy(gameObject);
        }
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

    IEnumerator inventoryFull()
    {
        inventorySystem.inventory.invFull.SetActive(true);
        yield return new WaitForSeconds(3f);
        inventorySystem.inventory.invFull.SetActive(false);
    }
}