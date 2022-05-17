using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IAltInteractable
{
    [Header("Chest Controls")]
    [SerializeField] bool m_open = false;
    [SerializeField] public GameObject m_lootPickup;
    
    
    [Header("References")]
    [SerializeField] Transform m_dropLocation;
    [SerializeField] GameObject m_closedModel;
    [SerializeField] GameObject m_openModel;

    private void Awake()
    {
        SwitchOpenStateModel(m_open);
    }

    public void AltInteract()
    {
        if(!m_open)
        {
            m_open = true;
            SwitchOpenStateModel(m_open);

            if(m_lootPickup != null && m_dropLocation != null)
            {
                Instantiate(m_lootPickup, m_dropLocation.position, m_dropLocation.rotation,transform.parent);
            }
        }
    }

    public InteractInfo CanInteract()
    {
        return new InteractInfo(!m_open,"OpenChest",2);
    }

    void SwitchOpenStateModel(bool open)
    {
        m_openModel.SetActive(open);
        m_closedModel.SetActive(!open);
    }
}
