using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungenManager : MonoBehaviour
{
    [SerializeField] Camera m_DungenCam;
    [SerializeField] float m_cameraSpeed;
    private Rigidbody m_CameraRB;


    private void Awake()
    {
        m_CameraRB = m_DungenCam.transform.GetComponent<Rigidbody>();
    }

    public IEnumerator MoveCameraCoroutine(Vector3 TargetLocation)
    {
        Vector3 toMoveCheck = TargetLocation - m_DungenCam.transform.position;
        while(Mathf.Pow(toMoveCheck.x, 2) + Mathf.Pow(toMoveCheck.z, 2) > 0.4)
        {
            Vector3 toMove = TargetLocation - m_DungenCam.transform.position;
            m_CameraRB.velocity = toMove.normalized * m_cameraSpeed;
            yield return new WaitForFixedUpdate();                
        }
        m_DungenCam.transform.position = TargetLocation;
    }
}
