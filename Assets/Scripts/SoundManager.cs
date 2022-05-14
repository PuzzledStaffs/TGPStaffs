using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    [FormerlySerializedAs("instance")]
    public static SoundManager m_instance;
    [FormerlySerializedAs("mixer")]
    public static AudioMixer m_mixer;

    //[SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        m_instance = this;
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
 
}
