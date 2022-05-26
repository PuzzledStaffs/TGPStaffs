using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldBridge : MonoBehaviour
{
    [SerializeField] private bool m_open;
    public Quaternion m_newRotation;


    public void OpenBridge()
    {
         transform.rotation = Quaternion.Euler(0,0,0);
         m_open = true;
    }

}
