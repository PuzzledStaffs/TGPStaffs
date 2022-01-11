using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public float maxAngle;
    public float distance;
    public float radius;
    public GameObject target;

    private void FixedUpdate()
    {
        
        Vector3 targetDirection = target.transform.position - transform.position;
        float angle = Vector3.Angle(targetDirection, transform.forward);


        //Gets all colliders within a radius around object
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var overlap in colliders)
        {
            //Checks if each collider is the target one
            if (overlap.transform == target.transform)
            {
                //if its within the angle
                if (angle < maxAngle)
                {
                    //Draws ray in front of object, if it collides with target object, then it can see the target
                    RaycastHit hit;

                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10))
                    {
                        if (hit.transform == target.transform)
                        {
                            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                            Debug.Log("Hit");
                        }
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                    }
                }
            }

        }

        
       

        /* Vector3 DirectionBetween = (target.position- checkObject.position).normalized;
                    DirectionBetween.y *= 0;

                    float angle = Vector3.Angle(checkObject.forward, DirectionBetween);

                    if (angle <= maxAngle)
                    {
                        Ray ray = new Ray(checkObject.position, target.position - checkObject.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, maxRadius))
                        {
                            if (hit.transform == target)
                            {
                              
                                return true;
                            }
                        }*/

    }
}
