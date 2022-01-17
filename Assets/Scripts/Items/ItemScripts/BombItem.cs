using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Bomb Item")]
public class BombItem : Item
{
    public GameObject bombItem;
    public float moveForce;


    public override void LeftClickAction(PlayerController pc)
    {
        //Create bomb - Instantiate it
        GameObject newBomb = Instantiate(bombItem, pc.m_model.transform.position+ new Vector3(0f,0.2f,0f) + pc.m_model.transform.forward, Quaternion.identity);

        //The add force to the bomb rigidbody
        newBomb.GetComponent<Rigidbody>().AddForce(pc.m_model.transform.forward * moveForce, ForceMode.Impulse);

        newBomb.GetComponent<Rigidbody>().AddForce(pc.m_model.transform.up * 2);

        newBomb.GetComponent<Bomb>().StartCoroutine(newBomb.GetComponent<Bomb>().ExplodeCoroutine(ItemDamage));
        
        //DO this in the facing direction of the player
        //pc.m_model.transform.forward - facing direction of player

    }

}
