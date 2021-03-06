using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Bomb Item")]
public class BombItem : Item
{
    public GameObject m_bombItem;
    public float m_moveForce;
    public float m_radius;
    public GameObject m_ExplosionPrefab;
    public AudioClip ExplosionSound;

    public override void LeftClickAction(PlayerController pc, bool attackAnim)
    {
        //Create bomb - Instantiate it
        pc.animator.SetTrigger("ThrowBomb");
        GameObject newBomb = Instantiate(m_bombItem, pc.m_model.transform.position+ new Vector3(0f,2.0f,0f) + pc.m_model.transform.forward, Quaternion.identity);
        newBomb.GetComponent<Bomb>().m_bombParent = this;

        //The add force to the bomb rigidbody
        newBomb.GetComponent<Rigidbody>().AddForce(pc.m_model.transform.forward * m_moveForce, ForceMode.Impulse);

        newBomb.GetComponent<Rigidbody>().AddForce(pc.m_model.transform.up * 2);

        newBomb.GetComponent<Bomb>().StartCoroutine(newBomb.GetComponent<Bomb>().ExplodeCoroutine(ItemDamage));       
        //DO this in the facing direction of the player
        //pc.m_model.transform.forward - facing direction of player

    }

}
