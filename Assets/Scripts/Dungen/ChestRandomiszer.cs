using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChestRandomiszer : MonoBehaviour
{
    [SerializeField] private GameObject m_baseObject;
    [SerializeField] private List<ChestRandomThing> m_toFill;

    [Header("Mimics")] 
    [SerializeField] private int m_numOfMimis;

    [SerializeField] private GameObject m_enemyParent;
    [SerializeField] private DungenRoom m_dungenRoom;


    void Start()
    {
        List<Chest> chests = new List<Chest>();
        chests.AddRange(gameObject.GetComponentsInChildren<Chest>());

        foreach (ChestRandomThing thing in m_toFill)
        {
            for (int i = 0; i < thing.m_amountToPlace; i++)
            {
                if (chests.Count == 0)
                    return; //No more chest to fill end the start function
                else
                {
                    int chestIndex = Random.Range(0, chests.Count);
                    chests[chestIndex].m_lootPickup = thing.m_item;
                    chests.RemoveAt(chestIndex);
                }
            }
        }

        if (chests.Count <= 0) 
            return;

        if (m_dungenRoom != null && m_enemyParent != null)
        {
            for (int i = 0; i < m_numOfMimis; i++)
            {
                if (chests.Count == 0)
                    return; //No more chest to fill end the start function
                else
                {
                    int chestIndex = Random.Range(0, chests.Count);
                    chests[chestIndex].m_isMimic = true;
                    chests[chestIndex].m_room = m_dungenRoom;
                    chests[chestIndex].m_enemyParent = m_enemyParent;
                    chests.RemoveAt(chestIndex);
                }
            }
        }

        if (chests.Count <= 0)
            return;

        foreach (Chest chest in chests)
        {
            chest.m_lootPickup = m_baseObject;
        }

    }

}

[System.Serializable]
public struct ChestRandomThing
{
    public GameObject m_item;
    public int m_amountToPlace;
}