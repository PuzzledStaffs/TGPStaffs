using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public BombItem m_bombParent;

    public IEnumerator ExplodeCoroutine(int itemDamage)
    {
        yield return new WaitForSeconds(2);

        foreach (Collider col in Physics.OverlapSphere(transform.position, m_bombParent.m_radius))
        {
            IHealth.Damage damage;
            damage.damageAmount = itemDamage;
            damage.type = IHealth.DamageType.BOMB;

            col.gameObject.GetComponent<IHealth>()?.TakeDamage(damage);
        }

        Destroy(gameObject);
        GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().PlayOneShot(m_bombParent.ExplosionSound);
        Instantiate(m_bombParent.m_ExplosionPrefab, transform.position, Quaternion.identity);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (m_bombParent == null)
            return;
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, m_bombParent.m_radius);
    }
#endif

}
