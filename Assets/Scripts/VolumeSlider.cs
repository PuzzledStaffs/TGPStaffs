using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [FormerlySerializedAs("slider")]
    [SerializeField] private Slider m_slider;


    // Start is called before the first frame update
    void Start()
    {
    }
    private void Update()
    {
        m_slider.onValueChanged.AddListener(val => SoundManager.m_instance.ChangeMasterVolume(val));
        Debug.Log("Volume changed to" + m_slider.value);
    }


}
