using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public float maxAngle;
    public float radius;
    public GameObject target;
    public bool inFOV = false;

    //Draws Debug lines
    public void OnDrawGizmos()
    {
        //Sphere
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);

        //Line forward
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * radius));

        //Boundaries
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(maxAngle, transform.up) * (transform.forward * radius)));
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(-maxAngle, transform.up) * (transform.forward * radius)));

        //green if detected
        if (inFOV)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        //detection line
        Gizmos.DrawLine(transform.position, ((target.transform.position - transform.position).normalized * radius) + transform.position);
    }

    private void FixedUpdate()
    {
        //calculate the distance between this and the target
        float distanceBetween = Vector3.Distance(transform.position, target.transform.position);

        //direction of the object
        Vector3 colliderDirection = (target.transform.position - transform.position).normalized;

        //Check if the target is in the radius
        if (distanceBetween <= radius)
        {
            //if its within the angle
            if (Vector3.Angle(transform.forward, colliderDirection) <= maxAngle)
            {
                RaycastHit hit;

                //Draws ray in front of object and returns the object in front
                if (Physics.Raycast(transform.position, colliderDirection, out hit, distanceBetween))
                {
                    //if the object is the target, return true
                    inFOV = hit.transform == target.transform;
                }
            }
            else
            {
                //reset bool
                inFOV = false;
            }
        }
        else
        {
            //reset bool
            inFOV = false;
        }
    }
}
