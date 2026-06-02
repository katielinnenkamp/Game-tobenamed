using UnityEngine;
using UnityEngine.SceneManagement;

public class FogWall : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;

    private void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoseGame();
        }
    }

    private void LoseGame()
    {
        SceneManager.LoadScene("FullGameLose");
    }
}