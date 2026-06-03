using UnityEngine;
using System.Collections.Generic;

public class elevatorbutton : Interactable
{
    public bool goesup;
    private AudioManager _audioManager;
    private AudioSource m_ButtonClick;

    [SerializeField]
    private string name;

    void Awake()
    {
        _audioManager = FindFirstObjectByType<AudioManager>();
        if (_audioManager == null) Debug.LogError("_audioManager is NULL");
        m_ButtonClick = GameObject.Find("Button").GetComponent<AudioSource>();
        if (m_ButtonClick == null) Debug.LogError("m_Menu is NULL");
    }

    public override void Interact(GameObject Player)
    {
        Player.TryGetComponent<playerMove>(out var playerscript);
        m_ButtonClick.Play();
        if(goesup){anomalymanager.instance.GoUp();}
        else{anomalymanager.instance.GoDown();}
    }

    public override string GetName()
    {
        return name;
    }
}