using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class inventorySystem : MonoBehaviour
{
    public static inventorySystem inventory;
    public GameObject interact;
    public GameObject invFull;
    public List<itemData> items = new List<itemData>();

    public Transform itemContent;
    public GameObject inventoryItem;

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
            GameObject obj = Instantiate(inventoryItem, itemContent);
            var itemName = obj.transform.Find("itemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("itemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }
}