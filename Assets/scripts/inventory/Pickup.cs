using UnityEngine;

public class Pickup : Interactable
{
    [SerializeField]
    private Item item_form;

    public bool Grab(Inventory target)
    {
        if(target.AddItem(item_form))
        {
            Destroy(gameObject);
            return true;
        }
        else
        {
            Debug.Log("inventory full");
            return false;
        }
    }
}
