using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public float DestroyAfter;

    private void Start()
    {
        StartCoroutine(DestroyParticle());
    }



    IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(DestroyAfter);
        Destroy(this.gameObject);
    }



}
