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
    protected SignCanvas m_signCanvasScript;
    private TextMeshProUGUI m_signTextText;
    private Canvas m_canvas;
    [FormerlySerializedAs("m_currentDungenRoom")]
    public DungenRoom m_currentDungeonRoom;
   
    protected bool m_reading;
    private PlayerController m_playerController;
    private PlayerInput m_playerInput;

    protected virtual void Start()
    {
        m_playerController = FindObjectOfType<PlayerController>();
        m_playerInput = FindObjectOfType<PlayerInput>();

        m_signCanvasScript = GameObject.FindGameObjectWithTag("SignCanvas").GetComponent<SignCanvas>();

        m_canvas = m_signCanvasScript.m_canvas;
        m_signTextText = m_signCanvasScript.m_textBody;


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

    public virtual InteractInfo CanInteract()
    {
        return new InteractInfo(true,"Read",1);
    }
}
