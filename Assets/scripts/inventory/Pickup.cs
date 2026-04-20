using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private Item item_form;

    public void Grab(Inventory target)
    {
        if(target.AddItem(item_form))
        {
            Debug.Log("picking up item");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("inventory full");
        }
    }
}
