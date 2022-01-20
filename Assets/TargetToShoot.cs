using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetToShoot : MonoBehaviour, IHealth
{
    public Material notShotMaterial;
    public Material yesShotMaterial;
    public float moveForce;

    [Header("Components")]
    private Renderer modelRenderer;


    void Start()
    {
        modelRenderer = gameObject.GetComponent<Renderer>();
    }

    public int GetHealth()
    {
        return 0;
    }

    public bool isDead()
    {
        return false;
    }

    public void TakeDamage(IHealth.Damage damage)
    {
        if (damage.type == IHealth.DamageType.BOW)
        {
            StartCoroutine(GetShotCoroutine());
        }
        else if(damage.type==IHealth.DamageType.BOMB)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.up * moveForce,ForceMode.Impulse);
        }
    }

    IEnumerator GetShotCoroutine()
    {
        modelRenderer.material = yesShotMaterial;
        yield return new WaitForSeconds(2);
        modelRenderer.material = notShotMaterial;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
