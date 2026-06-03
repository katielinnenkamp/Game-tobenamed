using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenu;
    public playerMove player;
    private AudioManager _audioManager;
    private AudioSource m_Menu;

    private bool paused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;        
        _audioManager = FindFirstObjectByType<AudioManager>();
        if (_audioManager == null) Debug.LogError("_audioManager is NULL");
        m_Menu = GameObject.Find("Menu").GetComponent<AudioSource>();
        if (m_Menu == null) Debug.LogError("m_Menu is NULL");

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (paused)
        {
            m_Menu.Play();
            ContinueGame();
        }
        else
        {
            m_Menu.Play();
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        paused = true;

        if (player != null)
        {
            player.UnlockCursor();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        paused = false;

        if (player != null)
        {
            player.LockCursor();
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        paused = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_Menu.Play();
        SceneManager.LoadScene("BetaBuildMenu");
    }
}
