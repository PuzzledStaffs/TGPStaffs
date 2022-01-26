using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_signText;
    private TextMeshProUGUI m_signTextText;
    private Canvas m_canvas;
    public DungenRoom m_currentDungenRoom;
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
        m_canvas.enabled = false;
    }

    public virtual void Interact()
    {
        if (!m_reading)
        {
            m_signTextText.text = m_signText;
            m_canvas.enabled = true;
            m_reading = true;
            m_playerController.enabled = false;
            if (m_currentDungenRoom != null)
            {
                m_currentDungenRoom.FrezzeExatingRoom();
            }

        }
        else
        {
            m_reading = false;
            m_canvas.enabled = false;
            m_playerController.enabled = true;
            if (m_currentDungenRoom != null)
            {
                m_currentDungenRoom.UnFrezeRoom();
            }
        }
    }
}