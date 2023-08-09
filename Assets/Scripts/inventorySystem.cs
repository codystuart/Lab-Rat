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

    //max inventory space
    public int maxItems = 9;

    //list of items
    public List<itemData> items = new List<itemData>();

    //the currently selected item
    public itemData selectedItem;

    //button creation info
    public Transform itemContent;
    public Button inventoryItem;

    public void Awake()
    {
        inventory = this;
    }

    public void Add(itemData item)
    {
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

        foreach (var item in items)
        {
            Button button = Instantiate(inventoryItem, itemContent);
            //GameObject obj = Instantiate(inventoryItem, itemContent);
            var itemName = button.transform.Find("itemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = button.transform.Find("itemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;

            button.onClick.AddListener(() => display(item));
        }
    }

    public void display(itemData currItem)
    {
        selectedItem = currItem;
        gameManager.instance.displayName.text = currItem.itemName;
        char currId = currItem.id;
        gameManager.instance.description.text = descriptionText(currId);
    }

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
        }
    }

    public void drop()
    {
        if (selectedItem != null)
        {

        }
    }

    public void itemFunction(itemData selectedItem)
    {
        char id = selectedItem.id;

        if (id == 'a')
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
            }
        } 
        if (id == 'b')
        {

        }
        if (id == 'c')
        {

        }
        if (id == 'h')
        {

        }
        if (id == 'k')
        {

        }

        ListItems();
    }

    IEnumerator noGun()
    {
        //tells player that they don't have a gun
        gameManager.instance.noGun.SetActive(true);
        yield return new WaitForSeconds(2);
        gameManager.instance.noGun.SetActive(false);
    }
}