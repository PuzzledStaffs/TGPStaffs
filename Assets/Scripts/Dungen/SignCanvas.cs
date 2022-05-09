using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignCanvas : MonoBehaviour
{
    [SerializeField] Canvas m_canvas;
    // Start is called before the first frame update
    void Start()
    {
        m_canvas.enabled = false;
    }


}
