using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField]private AudioMixer audioMixer;
    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(level) * 20f);
    }
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(level) * 20f);
    }
    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(level) * 20f);
    }
}