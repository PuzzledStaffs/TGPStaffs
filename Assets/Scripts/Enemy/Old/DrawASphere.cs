using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawASphere : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, 2);
    }
}
