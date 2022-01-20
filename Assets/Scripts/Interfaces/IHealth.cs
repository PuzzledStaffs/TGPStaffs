using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    enum  DamageType
    { 
        SWORD,
        BOW,
        ENEMY,
    }

    public struct Damage
    {
        public DamageType type;
        public int damageAmount;
    }

    public int GetHealth();
    public void TakeDamage(Damage damage);
    public bool isDead();

}


