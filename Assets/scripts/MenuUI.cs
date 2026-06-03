using UnityEngine;

public class MenuUI : MonoBehaviour
{
    private AudioManager _audioManager;
    private AudioSource m_Menu;

    void Awake()
    {
        _audioManager = FindFirstObjectByType<AudioManager>();
        if (_audioManager == null) Debug.LogError("_audioManager is NULL");
        m_Menu = GameObject.Find("Menu").GetComponent<AudioSource>();
        if (m_Menu == null) Debug.LogError("m_Menu is NULL");
        _audioManager.musToPlay("Main");
    }
    public void PlayFullGame()
    {
        m_Menu.Play();
        GameFlowManager.instance.StartFullGame();
    }

    public void PlayGhostHouseOnly()
    {
        m_Menu.Play();        
        GameFlowManager.instance.PlayGhostHouseOnly();
    }

    public void PlayAnomalyGameOnly()
    {
        m_Menu.Play();
        GameFlowManager.instance.PlayAnomalyGameOnly();
    }

    public void PlayPlatformerGameOnly()
    {       
        m_Menu.Play();
        GameFlowManager.instance.PlayPlatformerGameOnly();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
