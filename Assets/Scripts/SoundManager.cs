using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public static AudioMixer mixer;

    //[SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
 
}
