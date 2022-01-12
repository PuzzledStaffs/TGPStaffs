using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public float maxAngle;
    public float distance;
    public float radius;
    public GameObject target;
    bool inFOV = false;


    private void FixedUpdate()
    {
        float distanceBetween = Vector3.Distance(transform.position, target.transform.position);
        //float angle = Vector3.Angle(transform.forward, distanceBetween);


        //Gets all colliders within a radius around object
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var overlap in colliders)
        {
            Vector3 colliderDirection = (overlap.transform.position - transform.position).normalized;
            //if its within the angle
            if (Vector3.Angle(transform.forward, colliderDirection) <= maxAngle)
                {
                //Draws ray in front of object, if it collides with target object, then it can see the target
                    Ray ray = new Ray(transform.forward, target.transform.position - transform.position);
                    RaycastHit hit;

                //  if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10))
                if (Physics.Raycast(transform.position, colliderDirection, out hit, distanceBetween))
                {
                    if (hit.transform == target.transform)
                    {
                        inFOV = true;
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                        Debug.Log("Hit");
                    }
                   
                }
                else
                {
                    inFOV = false;
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                }


            }


        }
    }
}
