using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Sword Item")]
public class SwordItem : Item
{

    public float SwordRange;
    public Color MainSwordTrailColor, SecondarySwordTrailColor;

    public override void LeftClickAction(PlayerController pc, bool attackAnim)
    {
        SwordAttack(pc, attackAnim);
    }


    public void SwordAttack(PlayerController pc, bool attackAnim)
    {
        IHealth.Damage damage = new IHealth.Damage();
        damage.type = IHealth.DamageType.SWORD;
        damage.damageAmount = ItemDamage;
        // Gets all objects with a collider in a box (halfExtents = scale / 2) in front of the player
        foreach (Collider col in Physics.OverlapBox(pc.transform.position + pc.m_model.transform.forward, new Vector3(SwordRange, SwordRange, SwordRange), pc.m_model.transform.rotation))
        {
            if (col.CompareTag("Player"))
                continue;
            col.GetComponent<IInteractable>()?.Interact();
            col.GetComponent<IHealth>()?.TakeDamage(damage);
        }

        pc.SwordTrailParticle.startColor = MainSwordTrailColor;
        pc.SecondarySwordTrail.startColor = SecondarySwordTrailColor;
        pc.Sword.SetActive(true);

        int r = Random.Range(1,3);

        if (attackAnim)
        {
            pc.animator.SetTrigger("AttackTrigger");

        }
        else
        {
            pc.animator.SetTrigger("AttackTrigger2");

        }
    }



}
