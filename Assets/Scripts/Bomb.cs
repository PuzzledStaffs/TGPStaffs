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
            if(col.tag == "Enemy")
            {
                col.gameObject.GetComponent<IInteractable>()?.Interact();
                col.gameObject.GetComponent<IHealth>()?.TakeDamage(BombParent.ItemDamage);
            }



            Debug.Log(col.name);
            col.GetComponent<IHealth>()?.TakeDamage(itemDamage);
        }

        Destroy(gameObject);
    }

 

}
