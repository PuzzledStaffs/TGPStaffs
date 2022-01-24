using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public enum  DamageType
    { 
        SWORD,
        BOW,
        ENEMY,
        BOMB,
        ENVIRONMENT
    }

    public struct Damage
    {
        public DamageType type;
        public int damageAmount;

        public Damage(DamageType type, int damage)
        {
            this.type = type;
            damageAmount =damage;
        }
    }

    public int GetHealth();
    public void TakeDamage(Damage damage);
    public bool isDead();

}


