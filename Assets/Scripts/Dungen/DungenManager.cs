using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungenManager : MonoBehaviour
{
    [SerializeField] Camera m_DungenCam;
    //[SerializeField] float m_cameraSpeed;
    float m_cameraTransitionTime = 1.0f;
    [SerializeField] TextMeshProUGUI m_KeyCountText;
    private Rigidbody m_CameraRB;
    public int m_KeysCollected { get; protected set; }
    [SerializeField] int m_StartingKeys;

    private void Awake()
    {
        m_CameraRB = m_DungenCam.transform.GetComponent<Rigidbody>();
        UpdateKeyUI();
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
    }
}
