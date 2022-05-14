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

    private void Awake()
    {
        m_instance = this;
        AudioListener.volume = PersistentPrefs.GetInstance().m_settings.m_volume;
    }

    public float GetMasterVolume()
    {
        return AudioListener.volume;
    }

    public void ChangeMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PersistentPrefs.GetInstance().m_settings.m_volume = volume;
        PersistentPrefs.GetInstance().SaveSettings();
    }
 
}
