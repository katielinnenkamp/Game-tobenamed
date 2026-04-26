using UnityEngine;

[CreateAssetMenu(fileName = "Orb", menuName = "Scriptable Objects/Orb")]
public class Orb : Item
{
    public override void Use(GameObject user)
    {
        Debug.Log("you have utilized the orb.");
        return;
    }
}
