using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Inventory
{
    private int slots; //maximum number of items that can be picked up

    private Dictionary<int, Item> items = new Dictionary<int, Item>();

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

    public bool SlotEmpty(int index)
    {
        return !items.ContainsKey(index);
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

    public void SwapSlots(int ind1, int ind2)
    {
        bool s1 = !this.SlotEmpty(ind1);
        bool s2 = !this.SlotEmpty(ind2);
        //first slot item, second slot empty
        if(s1 && !s2)
        {
            items[ind2] = this.RemoveItem(ind1);
        }
        //first slot empty, second slot item
        else if(!s1 && s2)
        {
            items[ind1] = this.RemoveItem(ind2);
        }
        //both slots full
        else if(s1 && s2)
        {
            Item temp = items[ind1];
            items[ind1] = items[ind2];
            items[ind2] = temp;
        }
        //otherwise, both slots are empty
        return;
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < slots; i++)
        {
            if (!items.ContainsKey(i))
            {
                items[i] = item;
                return true;
            }
        }
        return false;
    }
    public bool TryGetItem(int index, out Item item)
    {
        return items.TryGetValue(index, out item);
    }
    public Item RemoveItem(int index)
    {
        Item ret = items[index];
        items.Remove(index);
        return ret;
    }
}
