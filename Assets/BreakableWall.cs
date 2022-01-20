using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IHealth
{
    public int GetHealth() { return 0; }
    public void TakeDamage(IHealth.Damage damage) 
    {
        //gameObject.SetActive(false);
        if (damage.type == IHealth.DamageType.BOMB)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
    public bool isDead() { return false; }
}
