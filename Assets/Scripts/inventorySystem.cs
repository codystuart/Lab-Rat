using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventorySystem : MonoBehaviour
{
    public static inventorySystem inventory;
    public List<itemData> items = new List<itemData>();

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
        items.Remove(item);
    }
}