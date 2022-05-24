using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Sword Item")]
public class SwordItem : Item
{

    public float SwordRange;
    public Color MainSwordTrailColor, SecondarySwordTrailColor;

    public static float m_nextFireTime = 0.0f;
    public static int m_noOfClicks = 0;
    public static float m_lastClickedTime = 0;
    public static float m_maxComboDelay = 1.0f;


    public override void LeftClickAction(PlayerController pc, bool attackAnim)
    {
        if (Time.time > m_nextFireTime)
        {
            SwordAttack(pc, attackAnim);
        }
    }


    public void SwordAttack(PlayerController pc, bool attackAnim)
    {
       // m_lastClickedTime = Time.time;
       // m_noOfClicks++;

        IHealth.Damage damage = new IHealth.Damage();
        damage.type = IHealth.DamageType.SWORD;
        damage.damageAmount = ItemDamage;
        // Gets all objects with a collider in a box (halfExtents = scale / 2) in front of the player
        foreach (Collider col in Physics.OverlapBox(pc.transform.position + pc.m_model.transform.forward, new Vector3(SwordRange, SwordRange, SwordRange), pc.m_model.transform.rotation))
        {
            if (col.CompareTag("Player"))
                continue;
            string name = col.transform.name;
            Debug.Log(name);
            col.GetComponent<IInteractable>()?.Interact();
            col.GetComponent<IHealth>()?.TakeDamage(damage);
        }

        pc.SwordTrailParticle.startColor = MainSwordTrailColor;
        pc.SecondarySwordTrail.startColor = SecondarySwordTrailColor;
        pc.Sword.SetActive(true);

        int r = Random.Range(1,3);

        if(attackAnim)
        {
            pc.animator.SetTrigger("AttackTrigger");

        }
        else
        {
            pc.animator.SetTrigger("AttackTrigger2");

        }


        //if (m_noOfClicks == 1)
        //{
        //    pc.SwordTrailParticle.startColor = MainSwordTrailColor;
        //    pc.SecondarySwordTrail.startColor = SecondarySwordTrailColor;
        //    pc.Sword.SetActive(true);
        //    pc.animator.SetBool("Attack1", true);

        //}
        //m_noOfClicks = Mathf.Clamp(m_noOfClicks, 0, 2);

        //if (m_noOfClicks >= 2 && pc.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && pc.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        //{
        //    pc.SwordTrailParticle.startColor = MainSwordTrailColor;
        //    pc.SecondarySwordTrail.startColor = SecondarySwordTrailColor;
        //    pc.animator.SetBool("Attack1", false);
        //    pc.animator.SetBool("Attack2", true);
        //}
    }



}
