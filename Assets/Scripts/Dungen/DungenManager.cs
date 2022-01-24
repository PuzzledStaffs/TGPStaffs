using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungenManager : MonoBehaviour
{
    [SerializeField] Camera m_DungenCam;
    //[SerializeField] float m_cameraSpeed;
    float m_cameraTransitionTime = 1.0f;
    [SerializeField] TextMeshProUGUI m_KeyCountText;
    [SerializeField] private Canvas m_keyCanvas;
    private Rigidbody m_CameraRB;
    public int m_KeysCollected { get; protected set; }
    [SerializeField] int m_StartingKeys;
    [Header("restart")]
    [SerializeField] PlayerController m_player;
    [SerializeField] string m_scene;
    [Header("Room Start Info")]
    [SerializeField] DungenRoom m_startingRoom;
    [SerializeField] string m_dungenEnterText;
    [SerializeField] Canvas m_welcomeCanvas;
    [SerializeField] TextMeshProUGUI m_TitalText;
    private Animator m_animator;

    private void Awake()
    {
        m_CameraRB = m_DungenCam.transform.GetComponent<Rigidbody>();
        m_KeysCollected = m_StartingKeys;
        UpdateKeyUI();
        if (m_KeysCollected == 0)
        {
            m_keyCanvas.enabled = false;
        }
        m_animator = GetComponent<Animator>();
        m_welcomeCanvas.enabled = false;
        m_player.m_Death += PlayerDeath;
    }

    private void Start()
    {
        GameObject.FindObjectOfType<PlayerController>().enabled = false;
        m_TitalText.text = m_dungenEnterText;
        m_welcomeCanvas.enabled = true;
        m_animator.SetTrigger("Start");
    }

    public void JoinAnimationEnd()
    {
        GameObject.FindObjectOfType<PlayerController>().enabled = true;
        m_startingRoom.StartRoom();
        m_welcomeCanvas.enabled = false;
    }

    private void PlayerDeath()
    {
        SceneManager.LoadScene(m_scene,LoadSceneMode.Single);
    }

    public IEnumerator MoveCameraCoroutine(Vector3 TargetLocation)
    {
        Vector3 initialPosition = m_DungenCam.transform.position;
        //Vector3 toMove = TargetLocation - m_DungenCam.transform.position;
        float time = 0.0f;
        //while (Mathf.Pow(toMove.x, 2) + Mathf.Pow(toMove.z, 2) > 0.4) //Mathf.Pow(toMove.x, 2) + Mathf.Pow(toMoveCheck.z, 2) > 0.4
        while (time < m_cameraTransitionTime) //Mathf.Pow(toMove.x, 2) + Mathf.Pow(toMoveCheck.z, 2) > 0.4
        {
            //toMove = TargetLocation - m_DungenCam.transform.position;
            m_DungenCam.transform.position = Vector3.Lerp(initialPosition, TargetLocation, time);
            //m_CameraRB.velocity = toMove.normalized * m_cameraSpeed;
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }
        m_CameraRB.velocity = Vector3.zero;
        m_DungenCam.transform.position = TargetLocation;
    }

    public void AddKey()
    {
        m_KeysCollected++;        
        UpdateKeyUI();
    }

    public bool UseKey()
    {
        if(m_KeysCollected > 0)
        {
            m_KeysCollected--;            
            UpdateKeyUI();
            return true;
        }
        else
        {
            return false;
        }
    }


    private void UpdateKeyUI()
    {
        m_KeyCountText.text = "x" + m_KeysCollected.ToString();
        if(m_KeysCollected > 0)
        {
            m_keyCanvas.enabled = true;
        }
        else
        {
            m_keyCanvas.enabled = false;
        }
    }
}
