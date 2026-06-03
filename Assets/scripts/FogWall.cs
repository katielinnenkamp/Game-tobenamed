using UnityEngine;

public class FogWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            parkourmanager.instance.LoseLife();
        }
    }
}