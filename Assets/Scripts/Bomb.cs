using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public BombItem BombParent;
    public IEnumerator ExplodeCoroutine(int itemDamage)
    {
        yield return new WaitForSeconds(2);

        foreach (Collider col in Physics.OverlapBox(transform.position, new Vector3(1.0f, 1.0f, 1.0f) * 1.5f))
        {
            IHealth.Damage damage;
            damage.damageAmount = itemDamage;
            damage.type = IHealth.DamageType.BOMB;
            if(col.tag == "Enemy")
            {
                col.gameObject.GetComponent<IInteractable>()?.Interact();
                col.gameObject.GetComponent<IHealth>()?.TakeDamage(damage);
            }



            Debug.Log(col.name);
            col.GetComponent<IHealth>()?.TakeDamage(damage);
        }

        Destroy(gameObject);
        Instantiate(BombParent.ExplosionPrefab, transform.position, Quaternion.identity);
    }
}
