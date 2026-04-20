using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Inventory
{
    private int slots; //maximum number of items that can be picked up

    private List<Item> items = new List<Item>();

    public Inventory()
    {
        slots = 1;
    }

    public Inventory(int initialslots)
    {
        slots = initialslots;
    }

    public int GetSlots()
    {
        return slots;
    }
    public int NumItems()
    {
        return items.Count;
    }
    //handles dropping excess items in case slots decreases and goes over the limit
    public void SetSlots(int newslots)
    {
        if(newslots < 0)
        {
            return;
        }
        slots = newslots;
        return;
    }

    public bool AddItem(Item item)
    {
        if(items.Count < slots)
        {
            items.Add(item);
            return true;
        }
        return false;
    }
    public Item GetItem(int index)
    {
        return items[index];
    }
    public void RemoveItem(int index)
    {
        items.Remove(items[index]);
    }
}
