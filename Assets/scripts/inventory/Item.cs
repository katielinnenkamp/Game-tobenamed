using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    public string item_name;
    [SerializeField]
    public GameObject item_prefab;
}
