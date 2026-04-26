using UnityEngine;
using System.Collections.Generic;

public class cauldron : Useable
{
    public List<Item> given = new List<Item>();
    [SerializeField]
    private Item finalpotion;

    public override void Activate(int keyused)
    {
        if(!given.Contains(keys[keyused].key))
        {
            given.Add(keys[keyused].key);

            if(given.Count == keys.Length)
            {
                Success();
            }
        }
    }

    public void Success()
    {
        Instantiate(finalpotion.item_prefab, 
                transform.position + new Vector3(-0.5f, 0f, 0f), 
                Quaternion.identity);
    }
}