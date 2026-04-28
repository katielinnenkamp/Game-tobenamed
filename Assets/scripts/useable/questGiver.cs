using UnityEngine;
using System.Collections.Generic;

public class questGiver : Useable
{
    public GameObject dialogBox;

    [SerializeField]
    private Item reward;

    public override void Activate(int keyused)
    {
        dialogBox.SetActive(true);
        Instantiate(reward.item_prefab, 
                transform.position + new Vector3(1f, 0f,-1f), 
                Quaternion.identity);
    }
}