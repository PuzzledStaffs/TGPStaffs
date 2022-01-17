using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public IEnumerator ExplodeCoroutine(int itemDamage)
    {
        yield return new WaitForSeconds(2);

        foreach (Collider col in Physics.OverlapBox(transform.position, new Vector3(1.0f, 1.0f, 1.0f)))
        {
            Debug.Log(col.name);
            col.GetComponent<IHealth>()?.TakeDamage(itemDamage);
        }

        Destroy(gameObject);
    }

}
