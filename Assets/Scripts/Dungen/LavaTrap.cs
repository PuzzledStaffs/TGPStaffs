using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTrap : MonoBehaviour
{
    [SerializeField] private bool m_AmBrigde;
    [SerializeField] private Material m_lavaMat;
    [SerializeField] private Material m_brigdeMat;
    private Renderer m_renderer;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
        if(m_AmBrigde)
        {
            BecomeBrigde();
        }
        else
        {
            BecomeLava();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_AmBrigde)
        {
            if (other.tag == "Player")
            {
                //Deal player damge
                other.transform.GetComponent<PlayerController>().Respawn();
            }
        }
    }

    public void BecomeBrigde()
    {
        m_AmBrigde = true;
        m_renderer.material = m_brigdeMat;
    }

    public void BecomeLava()
    {
        m_AmBrigde = false;
        m_renderer.material = m_lavaMat;
    }
}
