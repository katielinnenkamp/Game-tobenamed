using UnityEngine;

public class boat : Interactable
{
    public override void Interact(GameObject Player)
    {
        parkourmanager.instance.WinGame();
    }

    public override string GetName()
    {
        return "Boat";
    }
}
