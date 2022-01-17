using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungenManager : MonoBehaviour
{
    [SerializeField] Camera m_DungenCam;
    //[SerializeField] float m_cameraSpeed;
    float m_cameraTransitionTime = 1.0f;
    private Rigidbody m_CameraRB;


    private void Awake()
    {
        m_CameraRB = m_DungenCam.transform.GetComponent<Rigidbody>();
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
}
