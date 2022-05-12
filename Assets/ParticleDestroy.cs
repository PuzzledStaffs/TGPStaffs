using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class ParticleDestroy : MonoBehaviour
{
    [FormerlySerializedAs("DestroyAfter")]
    public float m_destroyAfter;

    private void Start()
    {
        StartCoroutine(DestroyParticle());
    }



    IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(m_destroyAfter);
        Destroy(this.gameObject);
    }



}
