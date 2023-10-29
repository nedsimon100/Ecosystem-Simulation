using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OPTIONSMENU : MonoBehaviour
{
    public AudioMixer Music;
    public AudioMixer SFX;
    public AudioMixer MA;
    public Slider MusicSlid;
    public Slider SFXSlid;
    public Slider MASlid;

    private void Awake()
    {
        MASlid.maxValue = 0f;
        MASlid.minValue = -80f;
        MusicSlid.maxValue = 0f;
        MusicSlid.minValue = -80f;
        SFXSlid.maxValue = 0f;
        SFXSlid.minValue = -80f;
    }
    public void Update()
    {
        MusicSlid.value = PlayerPrefs.GetFloat("musicVOL");
        MASlid.value = PlayerPrefs.GetFloat("maVOL");
        SFXSlid.value = PlayerPrefs.GetFloat("sfxVOL");
        Music.SetFloat("MUSICVOL", PlayerPrefs.GetFloat("musicVOL"));
        SFX.SetFloat("SFXVOL", PlayerPrefs.GetFloat("maVOL"));
        MA.SetFloat("MAVOL", PlayerPrefs.GetFloat("sfxVOL"));
    }
    public void MusicVol(float Vol)
    {
        Music.SetFloat("MUSICVOL",Vol);
        PlayerPrefs.SetFloat("musicVOL", Vol);
    }
    public void SFXVol(float Vol)
    {
        SFX.SetFloat("SFXVOL",Vol);
        PlayerPrefs.SetFloat("sfxVOL", Vol);
    }
    public void MAVol(float Vol)
    {
        MA.SetFloat("MAVOL",Vol);
        PlayerPrefs.SetFloat("maVOL", Vol);
    }

}
