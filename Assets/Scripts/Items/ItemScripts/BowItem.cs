using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Bow Item")]
public class BowItem : Item
{
    public float CurrentRange;
    public float ArrowSpeed;
    public float MaxRange;
    public float BowSpeedMultiplier;
    bool PlayOnce = true;
    public AudioClip ReleaseSound;
    public GameObject Arrow;

    public override void LeftClickAction(PlayerController pc)
    {
        if (PlayOnce)
        {
            pc.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemSound);
            PlayOnce = false;
        }

        if(CurrentRange < MaxRange)
        {
            CurrentRange += Time.deltaTime * BowSpeedMultiplier;
        }


        Debug.DrawRay(pc.transform.position, pc.m_model.transform.forward * CurrentRange, Color.red);
        Debug.Log("BOW FIRE!");
    }

    public override void ReleaseAction(PlayerController pc)
    {
        if(CurrentRange > 2)
        {
            GameObject arrow = Instantiate(Arrow, pc.transform.position, pc.m_model.transform.rotation);
            arrow.GetComponent<Arrow>().bowParent = this;
            arrow.GetComponent<Arrow>().pc = pc;
            arrow.GetComponent<Arrow>().EndPoint = pc.transform.position + pc.m_model.transform.forward * CurrentRange;
            CurrentRange = 0;
            PlayOnce = true;
            pc.gameObject.GetComponent<AudioSource>().PlayOneShot(ReleaseSound);
            Debug.Log("BOW RELEASE!");
        }
    }


}

