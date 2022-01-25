using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGizmo : MonoBehaviour
{

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.black;
        Vector3 Range = new Vector3(1.0f, 1.0f, 1.0f) * 1.5f;
        Gizmos.DrawWireSphere(transform.position, Range.magnitude);
    }



}
