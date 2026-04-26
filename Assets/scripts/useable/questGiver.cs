using UnityEngine;
using System.Collections.Generic;

public class questGiver : Useable
{
    [SerializeField]
    private Item reward;

    public override void Activate(int keyused)
    {
        Instantiate(reward.item_prefab, 
                transform.position + new Vector3(0.5f, 0f, 0.5f), 
                Quaternion.identity);
    }
}