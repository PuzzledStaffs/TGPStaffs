using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] Vector2Int m_rangeOfCoinsSpawned;
    public AudioClip m_pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().m_audioSource.PlayOneShot(m_pickupSound);
            other.gameObject.GetComponent<PlayerController>().AddCoins(Random.Range(m_rangeOfCoinsSpawned.x, m_rangeOfCoinsSpawned.y));
            Destroy(gameObject);
        }
    }

    
}
