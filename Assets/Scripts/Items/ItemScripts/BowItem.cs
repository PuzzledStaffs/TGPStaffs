using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Bow Item")]
public class BowItem : Item
{
    float StartingRange = 4;
    public float CurrentRange;
    public float ArrowSpeed;
    public float MaxRange;
    public float BowSpeedMultiplier;
    bool PlayOnce = true;
    public AudioClip ReleaseSound;
    public GameObject Arrow;
    public Transform spawnPoint;

    private void Awake()
    {
        CurrentRange = StartingRange;     
    }



    public override void LeftClickAction(PlayerController pc)
    {
        pc.Bow.SetActive(true);
        pc.animator.SetBool("BowDraw", true);
        pc.animator.SetFloat("BowWalkSpeed", pc.m_rigidbody.velocity.magnitude / 10);
        Debug.Log(pc.m_rigidbody.velocity.magnitude / 10);
        if (PlayOnce)
        {
           // pc.animator.SetBool("BowDraw", true);
            pc.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemSound);
            PlayOnce = false;
        }

        if(CurrentRange < MaxRange)
        {
            CurrentRange += Time.deltaTime * BowSpeedMultiplier;
        }

        pc.BowLineRenderer.SetPosition(1, new Vector3(0, 0, CurrentRange - StartingRange));
        pc.BowLineRenderer.gameObject.transform.rotation = pc.m_model.transform.rotation;
        Debug.DrawRay(pc.transform.position, pc.m_model.transform.forward * CurrentRange, Color.red);
        Debug.Log("BOW FIRE!");
    }

    public override void ReleaseAction(PlayerController pc)
    {
        pc.Bow.SetActive(false);
        pc.animator.SetBool("BowDraw", false);
        pc.BowLineRenderer.SetPosition(1, new Vector3(0, 0, 0));
        GameObject arrow = Instantiate(Arrow, pc.spawnPoint.position, pc.spawnPoint.rotation);
        arrow.GetComponent<Arrow>().bowParent = this;
        arrow.GetComponent<Arrow>().pc = pc;
        arrow.GetComponent<Arrow>().EndPoint = pc.transform.position + pc.m_model.transform.forward * CurrentRange;
        CurrentRange = StartingRange;
        PlayOnce = true;
        pc.gameObject.GetComponent<AudioSource>().PlayOneShot(ReleaseSound);
        Debug.Log("BOW RELEASE!");
    }
}

