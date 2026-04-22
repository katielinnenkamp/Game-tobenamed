using UnityEngine;

[System.Serializable]
public struct key_use_pair
{
    public Item key;
    public bool consumed;
}

public abstract class Interactable : MonoBehaviour
{
    [SerializeField]
    protected key_use_pair[] keys;
    [SerializeField]
    protected bool no_key;

    public bool Interact(Item useditem)
    {
        for(int i = 0; i < keys.Length; i++)
        {
            if(useditem == keys[i].key)
            {
                Activate(i);
                return true;
            }
        }
        if(no_key)
        {
            Activate(-1);
            return true;
        }   
        
        return false;
    }

    public abstract void Activate(int keyused);
}