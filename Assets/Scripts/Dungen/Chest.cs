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

    [Header("Mimic")]
    [Tooltip("The ability to be a mimi is controlled by chest randomizer")]
    [SerializeField] private GameObject m_mimicPrefab;
     public bool m_isMimic;
    
    public GameObject m_enemyParent;
    public DungenRoom m_room;

    private void Awake()
    {
        SwitchOpenStateModel(m_open);
        m_isMimic = false;
    }

    public void AltInteract()
    {
        if(!m_open)
        {
            m_open = true;

            if (m_isMimic)
            {
                m_closedModel.SetActive(false);
                EnemyController mimic =  Instantiate(m_mimicPrefab, transform.position, transform.rotation,
                    m_enemyParent.transform).GetComponent<EnemyController>();
                m_room.AddEnemy(mimic);
                mimic.ChangeState(State.StateType.CHASE);
                mimic.name = "mimicOf" + gameObject.name;
                return;
            }

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
