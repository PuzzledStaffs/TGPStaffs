using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public BowItem bowParent;
    public PlayerController pc;
    public Vector3 EndPoint;
 


    private void Start()
    {
        StartCoroutine(EnableCollider());
    }


    void Update()
    {
        var Distance = Vector3.Distance(transform.position, EndPoint);
        Debug.LogWarning("Distance: " + Distance);
        if (Distance > 1.6f)
        {
            float speed = (bowParent.ArrowSpeed + bowParent.CurrentRange) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(EndPoint.x,transform.position.y,EndPoint.z), speed);
        }
        else if(Distance <= 1.6f)
        {
            KillArrow();
        }
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider>().enabled = true;
    }

    public void KillArrow()
    {
        Destroy(this.gameObject);
    }



    private void OnCollisionEnter(Collision collision)
    {
        IHealth.Damage damage = new IHealth.Damage();
        damage.damageAmount = bowParent.ItemDamage;
        damage.type = IHealth.DamageType.BOW;

        collision.gameObject.GetComponent<IInteractable>()?.Interact();
        collision.gameObject.GetComponent<IHealth>()?.TakeDamage(damage);
        KillArrow();
    }
}
