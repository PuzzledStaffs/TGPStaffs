using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TargetToShoot : MonoBehaviour, IHealth
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private bool m_shotOnce;
    private bool m_shot;

    public UnityEvent m_shotEvent;

    void Start()
    {
        //m_modelRenderer = gameObject.GetComponent<Renderer>();
    }

    public int GetHealth()
    {
        return 0;
    }

    public bool IsDead()
    {
        return false;
    }

    public void TakeDamage(IHealth.Damage damage)
    {
        if (damage.type == IHealth.DamageType.BOW)
        {
            //Play spin anumation
            m_animator.SetTrigger("Shot");
            //Call shot
            m_shotEvent?.Invoke();
        }
       
    }

   

}
