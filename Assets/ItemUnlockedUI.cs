using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUnlockedUI : MonoBehaviour
{

    bool animate = false;
    public void ItemUnlocked()
    {
        animate = true;
        this.gameObject.SetActive(true);
        StartCoroutine(Cooldown());
    }

    public void Update()
    {
        if (animate) 
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1,1,1), Time.deltaTime * 2);
        }
    }


    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
        animate = false;
        transform.localScale = Vector3.zero;
    }

}
