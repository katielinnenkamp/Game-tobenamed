using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    [Header("Walking")]
    [SerializeField] private AudioSource m_woodwalk;
    [SerializeField] private AudioSource m_carpetwalk;
    [SerializeField] private AudioSource m_hardwalk;
    [SerializeField] private AudioSource m_grasswalk;

    [Header("Light")]
    [SerializeField] private AudioSource m_flashlightON;
    [SerializeField] private AudioSource m_flashlightOFF;

    [Header("Pickups")]
    [SerializeField] private AudioSource m_pickupSource;
    [SerializeField] private AudioClip[] m_pickupClips;

    [Header("Drops")]
    [SerializeField] private AudioSource m_dropSource;
    [SerializeField] private AudioClip[] m_dropClips;

    [Header("Door")]
    [SerializeField] private AudioSource m_DoorOpen;
    [SerializeField] private AudioSource m_DoorClose;

    [Header("Misc")]
    [SerializeField] private AudioSource m_Splash; 
    [SerializeField] private AudioSource m_MenuMove;    
    [SerializeField] private AudioSource m_ButtonClick;
    [SerializeField] private AudioSource m_Cheer;

    [Header("Music")]
    [SerializeField] private AudioSource bgm_Title;
    [SerializeField] private AudioSource bgm_Win;
    [SerializeField] private AudioSource bgm_Cabin;
    [SerializeField] private AudioSource bgm_Parkour;
    [SerializeField] private AudioSource bgm_Anomaly;
    [SerializeField] private AudioSource bgm_Lose;

    [Header("Ghost")]
    [SerializeField] private AudioSource m_GhostFloat;
    [SerializeField] private AudioSource m_GhostTalk;

    void Awake()
    {
        StopWalking();
    }

    public void PlayRandomPickup()
    {
        //Set up and play an RNG clip index for our Pickup Source
        int clip = Random.Range(0, m_pickupClips.Length);
        m_pickupSource.pitch = Random.Range(0.9f, 1.1f);
        m_pickupSource.PlayOneShot(m_pickupClips[clip]);

    }

    public void PlayRandomDrop()
    {
        //Set up and play an RNG clip index for our Drop Source
        int clip = Random.Range(0, m_dropClips.Length);
        m_dropSource.pitch = Random.Range(0.9f, 1.1f);
        m_dropSource.PlayOneShot(m_dropClips[clip]);

    }

    public void DoorClopen(bool o)
    {
        if (o)
        {
            m_DoorClose.Play();
        }
        else
        {
            m_DoorOpen.Play();
        }
    }

    public void PlayWalking(string groundType)
    {

        AudioSource toPlay = groundType switch
        {
            "Wood"   => m_woodwalk,
            "Carpet" => m_carpetwalk,
            "Stone"   => m_hardwalk,
            "Grass"  => m_grasswalk,
            "Untagged" => m_hardwalk,
            _        => m_hardwalk  // fallback
        };

        if (toPlay == null)
        {
            return;
        }

        StopWalkingExcept(toPlay);

        if(!toPlay.isPlaying)
        {
            toPlay.Play();
        }
    }

    private void StopWalkingExcept(AudioSource sourceToKeep)
    {
        if(m_woodwalk != null && m_woodwalk != sourceToKeep) m_woodwalk.Stop();
        if(m_carpetwalk != null && m_carpetwalk != sourceToKeep) m_carpetwalk.Stop();
        if(m_hardwalk != null && m_hardwalk != sourceToKeep) m_hardwalk.Stop();
        if(m_grasswalk != null && m_grasswalk != sourceToKeep) m_grasswalk.Stop();
    }

    public void StopWalking()
    {
        m_woodwalk.Stop();
        m_carpetwalk.Stop();
        m_hardwalk.Stop();
        m_grasswalk.Stop();
    }

    public void PauseWalking()
    {
        m_woodwalk.Pause();
        m_carpetwalk.Pause();
        m_hardwalk.Pause();
        m_grasswalk.Pause();
    }

    public void Talk()
    {
        m_GhostTalk.pitch = Random.Range(0.7f, 1.2f);
        m_GhostTalk.Play();
    }


    public void musToPlay(string Scene)
    {
        
        AudioSource toPlay = Scene switch
        {
            "Main"   => bgm_Title,
            "Cabin" => bgm_Cabin,
            "Anomaly"   => bgm_Anomaly,
            "Parkour"  => bgm_Parkour,
            "Win" => bgm_Win,
            "Lose" => bgm_Lose,
            _        => bgm_Title  // fallback
        };

        if (toPlay == null)
        {
            return;
        }

        musToStop(toPlay);

        if(!toPlay.isPlaying)
        {
            toPlay.Play();
        }
    }
    private void musToStop(AudioSource sourceToKeep)
    {
        if(bgm_Title != null && bgm_Title != sourceToKeep) bgm_Title.Stop();
        if(bgm_Cabin != null && bgm_Cabin != sourceToKeep) bgm_Cabin.Stop();
        if(bgm_Anomaly != null && bgm_Anomaly != sourceToKeep) bgm_Anomaly.Stop();
        if(bgm_Parkour != null && bgm_Parkour != sourceToKeep) bgm_Parkour.Stop();
        if(bgm_Win != null && bgm_Win != sourceToKeep) bgm_Win.Stop();
        if(bgm_Lose != null && bgm_Lose != sourceToKeep) bgm_Lose.Stop();
    }

}