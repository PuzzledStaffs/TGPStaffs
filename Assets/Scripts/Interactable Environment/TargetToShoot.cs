using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TargetToShoot : MonoBehaviour, IHealth
{
    [FormerlySerializedAs("notShotMaterial")]
    public Material m_notShotMaterial;
    [FormerlySerializedAs("yesShotMaterial")]
    public Material m_yesShotMaterial;
    [FormerlySerializedAs("moveForce")]
    public float m_moveForce;

    [Header("Components")]
    [FormerlySerializedAs("modelRenderer")]
    private Renderer m_modelRenderer;


    void Start()
    {
        m_modelRenderer = gameObject.GetComponent<Renderer>();
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
            StartCoroutine(GetShotCoroutine());
        }
        else if(damage.type==IHealth.DamageType.BOMB)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.up * m_moveForce,ForceMode.Impulse);
        }
    }

    IEnumerator GetShotCoroutine()
    {
        m_modelRenderer.material = m_yesShotMaterial;
        yield return new WaitForSeconds(2);
        m_modelRenderer.material = m_notShotMaterial;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
