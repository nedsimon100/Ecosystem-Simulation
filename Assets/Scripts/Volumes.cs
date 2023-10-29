using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class Volumes : MonoBehaviour
{

    public AudioMixer Music;
    public AudioMixer SFX;
    public AudioMixer MA;

    public AudioSource MusicPlayer;
    public AudioClip[] Song;
    // Use this for initialization
    void Awake()
    {
        playRandomMusic();
    }

    void FixedUpdate()
    { 
        if (!MusicPlayer.isPlaying||Input.GetKeyDown("p"))
        { 
             playRandomMusic();
        }
      
        Music.SetFloat("MUSICVOL", PlayerPrefs.GetFloat("musicVOL"));
        SFX.SetFloat("SFXVOL", PlayerPrefs.GetFloat("maVOL"));
        MA.SetFloat("MAVOL", PlayerPrefs.GetFloat("sfxVOL"));
    }

    public void playRandomMusic()
    {
        MusicPlayer.clip = Song[Random.Range(0, Song.Length)] as AudioClip;
        MusicPlayer.Play();
    }
}



