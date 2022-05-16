using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KickCube : MonoBehaviour
{
    public event Action<Collider> m_onRemoveSelf;
    // Start is called before the first frame update
    private void OnDestroy()
    {
        m_onRemoveSelf?.Invoke(GetComponent<Collider>());
        
    }
}
