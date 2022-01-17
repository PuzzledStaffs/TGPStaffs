using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public int GetHealth();
    public void TakeDamage(int damage);
    public bool isDead(int health);

}
