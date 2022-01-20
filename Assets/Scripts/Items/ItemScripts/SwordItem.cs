using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Sword Item")]
public class SwordItem : Item
{

    public float SwordRange;

    public override void LeftClickAction(PlayerController pc)
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
        pc.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemSound);
        pc.FreezeMovement();
        pc.Sword.SetActive(true);
        pc.Sword.GetComponent<Animator>().SetTrigger("SwordAttack");
    }
}
