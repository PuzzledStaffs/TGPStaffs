using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour,IHealth
{
    public int GetHealth() { return 0; }
    public void TakeDamage(int damage) 
    {           
        //gameObject.SetActive(false);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    public bool isDead(int health) { return false; }
}
