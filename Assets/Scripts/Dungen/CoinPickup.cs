using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] Vector2Int m_rangeOfCoinsSpawned;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().AddCoins(Random.Range(m_rangeOfCoinsSpawned.x, m_rangeOfCoinsSpawned.y));
            Destroy(gameObject);
        }
    }

    
}
