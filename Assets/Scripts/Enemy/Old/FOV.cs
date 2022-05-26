using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class FOV : MonoBehaviour
{
    [FormerlySerializedAs("maxAngle")]
    public float m_maxAngle;
    [FormerlySerializedAs("radius")]
    public float m_radius;
    [FormerlySerializedAs("target")]
    public GameObject m_target;
    [FormerlySerializedAs("inFOV")]
    public bool m_inFOV = false;


    private void Start()
    {
       m_target = GameObject.FindGameObjectWithTag("Player");
    }
    //Draws Debug lines
    public void OnDrawGizmos()
    {
        //Sphere
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_radius);

        //Line forward
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * m_radius));

        //Boundaries
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(m_maxAngle, transform.up) * (transform.forward * m_radius)));
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(-m_maxAngle, transform.up) * (transform.forward * m_radius)));

        //green if detected
        if (m_inFOV)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        //detection line
        if(m_target != null)
            Gizmos.DrawLine(transform.position, ((m_target.transform.position - transform.position).normalized * m_radius) + transform.position);
    }
    private void FixedUpdate()
    {
        //calculate the distance between this and the target
        float distanceBetween = Vector3.Distance(transform.position, m_target.transform.position);

        //direction of the object
        Vector3 colliderDirection = (m_target.transform.position - transform.position).normalized;

        //Check if the target is in the radius
        if (distanceBetween <= m_radius)
        {
            //if its within the angle
            if (Vector3.Angle(transform.forward, colliderDirection) <= m_maxAngle)
            {
                RaycastHit hit;

                //Draws ray in front of object and returns the object in front
                if (Physics.Raycast(transform.position, colliderDirection, out hit, distanceBetween))
                {
                    //if the object is the target, return true
                    m_inFOV = hit.transform == m_target.transform;
                }
            }
            else
            {
                //reset bool
                m_inFOV = false;
            }
        }
        else
        {
            //reset bool
            m_inFOV = false;
        }
    }
}
