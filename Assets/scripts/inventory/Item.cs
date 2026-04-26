using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    public string item_name;
    [SerializeField]
    public GameObject item_prefab;
    public Sprite icon;

    public virtual void Use(GameObject user)
    {
        return;
    }
}
