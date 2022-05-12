using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Sign : MonoBehaviour, IAltInteractable
{
    [SerializeField] [TextArea] private string m_signText;
    private TextMeshProUGUI m_signTextText;
    private Canvas m_canvas;
    [FormerlySerializedAs("m_currentDungenRoom")]
    public DungenRoom m_currentDungeonRoom;
   
    private bool m_reading;
    private PlayerController m_playerController;
    private PlayerInput m_playerInput;

    private void Start()
    {
        m_playerController = FindObjectOfType<PlayerController>();
        m_playerInput = FindObjectOfType<PlayerInput>();

        m_canvas = GameObject.FindGameObjectWithTag("SignCanvas").GetComponent<Canvas>();
        m_signTextText = GameObject.FindGameObjectWithTag("SignText").GetComponent<TextMeshProUGUI>();


        m_reading = false;
        //m_canvas.enabled = false;
       
    }

    public virtual void AltInteract()
    {
        if (!m_reading)
        {
            m_signTextText.text = m_signText;
            m_canvas.enabled = true;
            m_reading = true;
            m_playerController.enabled = false;
            if (m_currentDungeonRoom != null)
            {
                m_currentDungeonRoom.FreezeExitingRoom();
            }

        }
        else
        {
            m_reading = false;
            m_canvas.enabled = false;
            m_playerController.enabled = true;
            if (m_currentDungeonRoom != null)
            {
                m_currentDungeonRoom.UnFrezeRoom();
            }
        }
    }

    public bool CanInteract()
    {
        return true;
    }
}
