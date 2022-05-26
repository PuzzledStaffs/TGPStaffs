using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class ItemUnlockedUI : MonoBehaviour
{
    bool m_animate = false;

    public void ItemUnlocked()
    {
        m_animate = true;
        this.gameObject.SetActive(true);
        StartCoroutine(Cooldown());
    }

    public void Update()
    {
        if (m_animate) 
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1,1,1), Time.deltaTime * 2);
        }
    }


    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
        m_animate = false;
        transform.localScale = Vector3.zero;
    }

}
