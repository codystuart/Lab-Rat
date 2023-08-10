using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    //inventory only (health, cure, ammo, batteries, keycard)
    [SerializeField] itemData data;

    //rotation info
    [SerializeField] Vector3 rotation;
    [SerializeField] float speed;

    bool canPickup;

    void Update()
    {
        //!! place glow function here after completed  (maybe remove rotation) !!//

        //item rotation
        transform.Rotate(rotation * speed * Time.deltaTime, Space.World);

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
            if (data.id == 'c')
                gameManager.instance.updateCureGameGoal(1);
            if (data.id == 'k')
                gameManager.instance.keycardAcquired = true;

            inventorySystem.inventory.Add(data);
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