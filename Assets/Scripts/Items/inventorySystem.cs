using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class inventorySystem : MonoBehaviour
{
    public static inventorySystem inventory;

    //ui stuff
    public GameObject interact;
    public GameObject invFull;
    public TextMeshProUGUI maxItemsLabel;
    public TextMeshProUGUI currHeldItems;

    //SFX
    [SerializeField] AudioSource pickupSound;
    [SerializeField] AudioSource dropSound;

    //max inventory space
    public int maxItems = 9;

    //list of items
    public List<itemData> items = new List<itemData>();
    public List<noteData> notes = new List<noteData>();

    //the currently selected item
    public itemData selectedItem;

    //inv button creation info
    public Transform itemContent;
    public Button inventoryButton;

    //note button creation info
    public Transform noteContent;
    public Button noteButton;

    //reference to play position
    public Transform player;

    public void Awake()
    {
        inventory = this;
        maxItemsLabel.text = maxItems.ToString();
    }

    public void addItem(itemData item)
    {
        pickupSound.Play();
        items.Add(item);
        currHeldItems.text = items.Count.ToString();
    }

    public void removeItem(itemData item)
    {
        if (items.Count > 0)
        {
            items.Remove(item);
        }
    }

    public void addNote(noteData note)
    {
        pickupSound.Play();
        notes.Add(note);
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
            Button button = Instantiate(inventoryButton, itemContent);
            var itemName = button.transform.Find("itemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = button.transform.Find("itemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;

            button.onClick.AddListener(() => displayItem(item));
        }
    }

    public void ListNotes()
    {
        //cleans content before opening
        foreach (Transform note in noteContent)
        {
            Destroy(note.gameObject);
        }

        //creates a button
        foreach (var note in notes)
        {
            Button button = Instantiate(noteButton, noteContent);
            var noteName = button.transform.Find("noteName").GetComponent<TextMeshProUGUI>();

            noteName.text = note.title;

            button.onClick.AddListener(() => displayNote(note));
        }
    }

    //displays item name
    public void displayItem(itemData currItem)
    {
        selectedItem = currItem;
        gameManager.instance.displayName.text = currItem.itemName;
        char currId = currItem.id;
        gameManager.instance.itemDescription.text = descriptionText(currId);
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

    //display note
    public void displayNote(noteData selectedNote)
    {
        gameManager.instance.noteDescription.text = selectedNote.note;
    }

    //button functions
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
            removeItem(selectedItem);
            selectedItem = null;
            ListItems();
        }
    }

    //tab button functions
    public void invTab()
    {
        //turn off notes, turn on inventory
        gameManager.instance.notes.SetActive(false);
        gameManager.instance.inventory.SetActive(true);
        gameManager.instance.activeMenu = gameManager.instance.inventory;
        ListItems();
    }

    public void notesTab()
    {
        //turn off inventory, turn on notes
        gameManager.instance.inventory.SetActive(false);
        gameManager.instance.notes.SetActive(true);
        gameManager.instance.activeMenu = gameManager.instance.notes;
        ListNotes();
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
        gameManager.instance.itemDescription.text = string.Empty;
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
            gameManager.instance.playerScript.fillAmmo();
            items.Remove(selectedItem);
            ListItems();
        }
    }

    //battery function
    public void refillBattery()
    {
        gameManager.instance.playerScript.replaceBattery(.4f);
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