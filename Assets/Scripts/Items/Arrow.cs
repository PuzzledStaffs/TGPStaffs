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

        if(Diff.magnitude < 1)
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
        collision.gameObject.GetComponent<IInteractable>()?.Interact();
        collision.gameObject.GetComponent<IHealth>()?.TakeDamage(bowParent.ItemDamage);
    }
}
