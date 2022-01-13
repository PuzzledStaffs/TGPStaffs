using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Sword Item")]
public class SwordItem : Item
{
    public override void LeftClickAction(PlayerController pc)
    {
        // Gets all objects with a collider in a box (halfExtents = scale / 2) in front of the player
        foreach (Collider col in Physics.OverlapBox(pc.transform.position + pc.m_model.transform.forward, new Vector3(1.0f, 1.0f, 1.0f) / 2, pc.m_model.transform.rotation))
        {
            if (col.CompareTag("Player"))
                continue;
            col.GetComponent<IInteractable>()?.Interact();
            col.GetComponent<IHealth>()?.TakeDamage(ItemDamage);
            pc.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemSound);
        }
    }


    public override void RightClickAction()
    {
        Debug.Log("SWORD SLASH RIGHT");
    }
}
