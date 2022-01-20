using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public BowItem bowParent;
    public PlayerController pc;
    public Vector3 EndPoint;
 

    void Update()
    {
        Debug.Log("Enpoint: " + EndPoint);
        if(transform.position != EndPoint)
        {
            float speed = (bowParent.ArrowSpeed + bowParent.CurrentRange) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, EndPoint, speed);
        }
        else if(transform.position == EndPoint)
        {
            KillArrow();
        }

        var Diff = transform.position - EndPoint;
        Debug.Log("Diff: " + Diff.normalized);



    }

    public void KillArrow()
    {
        GameObject.Destroy(this);
    }



    private void OnCollisionEnter(Collision collision)
    {
        IHealth.Damage damage = new IHealth.Damage();
        damage.damageAmount = bowParent.ItemDamage;
        damage.type = IHealth.DamageType.BOW;

        collision.gameObject.GetComponent<IInteractable>()?.Interact();
        collision.gameObject.GetComponent<IHealth>()?.TakeDamage(damage);
    }
}
