using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager instance;

    public bool playingFullGame = false;
    public bool completedAnomalyGame = false;
    public bool hasOutsideKey = false;
    private AudioManager _audioManager;
    private string _pendingMusic = null;

    private AudioManager GetAudio()
    {
        if (_audioManager == null)
            _audioManager = FindFirstObjectByType<AudioManager>();
        return _audioManager;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // After a scene loads, re-find AudioManager and play pending music
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _audioManager = null; // Clear stale reference
        if (_pendingMusic != null)
        {
            GetAudio()?.musToPlay(_pendingMusic);
            _pendingMusic = null;
        }
    }

    private void LoadSceneWithMusic(string scene, string music)
    {
        _pendingMusic = music;
        SceneManager.LoadScene(scene);
    }

    public void StartFullGame()
    {
        playingFullGame = true;
        completedAnomalyGame = false;
        hasOutsideKey = false;
        LoadSceneWithMusic("AlphaBuild", "Cabin");
    }

    public void PlayGhostHouseOnly()
    {
        playingFullGame = false;
        LoadSceneWithMusic("AlphaBuild", "Cabin");
    }

    public void PlayAnomalyGameOnly()
    {
        playingFullGame = false;
        LoadSceneWithMusic("anomaly", "Anomaly");
    }

    public void PlayPlatformerGameOnly()
    {
        playingFullGame = false;
        LoadSceneWithMusic("parkour", "Parkour");
    }

    public void EnterAnomalyGame()
    {
        LoadSceneWithMusic("anomaly", "Anomaly");
    }

    public void CompleteAnomalyGame()
    {
        completedAnomalyGame = true;
        LoadSceneWithMusic("parkour", "Parkour");
    }

    public void StartPlatformerGame()
    {
        LoadSceneWithMusic("parkour", "Parkour");
    }

    public void WinGame()
    {
        LoadSceneWithMusic("FullGameWin", "Win");
    }

    public void LoseGame()
    {
        LoadSceneWithMusic("FullGameLose", "Lose");
    }

    public void ReturnToMainMenu()
    {
        playingFullGame = false;
        completedAnomalyGame = false;
        hasOutsideKey = false;
        LoadSceneWithMusic("BetaBuildMenu", "Main"); // "Title" → "Main" to match your switch
    }
}
