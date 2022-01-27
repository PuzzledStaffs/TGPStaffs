using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public float Amount, Speed;
    void Update()
    {
        gameObject.transform.Rotate(0, Amount * Time.deltaTime * Speed, 0);
    }
}
