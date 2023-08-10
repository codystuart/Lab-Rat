using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;

public class inventorySystem : MonoBehaviour
{
    public static inventorySystem inventory;

    //ui stuff
    public GameObject interact;
    public GameObject invFull;

    //SFX
    [SerializeField] AudioSource pickupSound;
    [SerializeField] AudioSource dropSound;

    //max inventory space
    public int maxItems = 9;

    //list of items
    public List<itemData> items = new List<itemData>();

    //the currently selected item
    public itemData selectedItem;

    //button creation info
    public Transform itemContent;
    public Button inventoryItem;

    //reference to play position
    public Transform player;

    public void Awake()
    {
        inventory = this;
    }

    public void Add(itemData item)
    {
        pickupSound.Play();
        items.Add(item);
    }

    public void Remove(itemData item)
    {
        if (items.Count > 0)
        {
            items.Remove(item);
        }
    }

    public void ListItems()
    {
        //cleans content before opening
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }

        //creates a button
        foreach (var item in items)
        {
            Button button = Instantiate(inventoryItem, itemContent);
            var itemName = button.transform.Find("itemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = button.transform.Find("itemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;

            button.onClick.AddListener(() => display(item));
        }
    }

    //displays item name
    public void display(itemData currItem)
    {
        selectedItem = currItem;
        gameManager.instance.displayName.text = currItem.itemName;
        char currId = currItem.id;
        gameManager.instance.description.text = descriptionText(currId);
    }

    //displays item description
    public string descriptionText(char id)
    {
        string theDescription = " ";

        if (id == 'a')
            theDescription = "Ammo pack restores current gun's ammunition.";
        if (id == 'b')
            theDescription = "Batteries recharge the flashlight.";
        if (id == 'c')
            theDescription = "This is the cure Miyah was talking about.";
        if (id == 'h')
            theDescription = "Health potion restores health.";
        if (id == 'k')
            theDescription = "This keycard should help me get around.";


        return theDescription;
    }

    public void use()
    {
        if (selectedItem != null)
        {
            itemFunction(selectedItem);
            clearInfo();
        }
    }

    public void drop()
    {
        if (selectedItem != null)
        {
            dropSound.Play();
            clearInfo();
            Vector3 playerPos = new Vector3(player.position.x - 3, player.position.y, player.position.z);
            Instantiate(selectedItem.prefab, playerPos, selectedItem.prefab.transform.rotation);
            Remove(selectedItem);
            selectedItem = null;
            ListItems();
        }
    }

    public void itemFunction(itemData selectedItem)
    {
        char id = selectedItem.id;

        if (id == 'a')
        {
            refillAmmo();
        } 
        if (id == 'b')
        {
            refillBattery();
        }
        if (id == 'h')
        {
            giveHealth(50);
        }
    }

    //clears item name and description
    public void clearInfo()
    {
        gameManager.instance.displayName.text = string.Empty;
        gameManager.instance.description.text = string.Empty;
    }

    //ammo function
    public void refillAmmo()
    {
        if (gameManager.instance.playerScript.gunList.Count <= 0)
        {
            StartCoroutine(noGun());
        }
        else if (gameManager.instance.playerScript.gunList[gameManager.instance.playerScript.selectedGun].currAmmo
            != gameManager.instance.playerScript.gunList[gameManager.instance.playerScript.selectedGun].maxAmmo)
        {
            gameManager.instance.playerScript.ammoPickup();
            items.Remove(selectedItem);
            ListItems();
        }
    }

    //battery function
    public void refillBattery()
    {
        items.Remove(selectedItem);
        ListItems();
    }

    //health function
    public void giveHealth(int amount)
    {
        if (gameManager.instance.playerScript.HP < gameManager.instance.playerScript.originalHP)
        {
            gameManager.instance.playerScript.HP += amount;
            gameManager.instance.playerScript.updatePlayerUI();
            items.Remove(selectedItem);
            ListItems();
        }
    }

    //tells player that they don't have a gun
    IEnumerator noGun()
    {
        gameManager.instance.noGun.SetActive(true);
        yield return new WaitForSeconds(2);
        gameManager.instance.noGun.SetActive(false);
    }
}