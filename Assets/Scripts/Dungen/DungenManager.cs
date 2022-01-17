using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungenManager : MonoBehaviour
{
    [SerializeField] Camera m_DungenCam;
    [SerializeField] float m_cameraSpeed;
    private Rigidbody m_CameraRB;
    public int m_KeysCollected { get; protected set; }

    private void Awake()
    {
        m_CameraRB = m_DungenCam.transform.GetComponent<Rigidbody>();
    }

    public IEnumerator MoveCameraCoroutine(Vector3 TargetLocation)
    {
        Vector3 toMove = TargetLocation - m_DungenCam.transform.position;
        while(Mathf.Pow(toMove.x, 2) + Mathf.Pow(toMove.z, 2) > 0.4) //Mathf.Pow(toMove.x, 2) + Mathf.Pow(toMoveCheck.z, 2) > 0.4
        {
            toMove = TargetLocation - m_DungenCam.transform.position;
            m_CameraRB.velocity = toMove.normalized * m_cameraSpeed;
            yield return new WaitForFixedUpdate();                
        }
        m_CameraRB.velocity = Vector3.zero;
        m_DungenCam.transform.position = TargetLocation;
    }

    public void AddKey()
    {
        m_KeysCollected++;
        //UpdateUI
    }

    public bool UseKey()
    {
        if(m_KeysCollected > 0)
        {
            m_KeysCollected--;
            //UpdateUI
            return true;
        }
        else
        {
            return false;
        }
    }

}
