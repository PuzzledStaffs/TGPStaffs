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


        if (PersistentPrefs.GetInstance().m_currentSaveFile.HasFlag(gameObject.scene.name + "_ChestsRandomised_" + gameObject.transform.parent.parent.name + "_" + gameObject.name))
        {
            for (int thing = 0; thing < m_toFill.Count; thing++)
            {
                for (int i = 0; i < m_toFill[thing].m_amountToPlace; i++)
                {
                    if (chests.Count == 0)
                        return;
                    int chestIndex = PersistentPrefs.GetInstance().m_currentSaveFile.GetIntFlag(gameObject.scene.name + "_ChestsRandomiser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name + "_Thing_" + thing + "_" + i);
                    chests[chestIndex].m_lootPickup = m_toFill[thing].m_item;
                    chests[chestIndex].m_chestIndex = chestIndex;
                    chests[chestIndex].m_flagPrefix = gameObject.scene.name + "_ChestsRandomiser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name;
                    chests.RemoveAt(chestIndex);
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
                        int chestIndex = PersistentPrefs.GetInstance().m_currentSaveFile.GetIntFlag(gameObject.scene.name + "_ChestsRandomiser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name + "_Mimic_" + i);
                        if (PersistentPrefs.GetInstance().m_currentSaveFile.HasFlag(gameObject.scene.name + "_ChestsRandomiser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name + "_MimicKilled_" + chestIndex))
                            chests[chestIndex].gameObject.SetActive(false);
                        chests[chestIndex].m_isMimic = true;
                        chests[chestIndex].m_room = m_dungenRoom;
                        chests[chestIndex].m_enemyParent = m_enemyParent;
                        chests[chestIndex].m_chestIndex = chestIndex;
                        chests[chestIndex].m_flagPrefix = gameObject.scene.name + "_ChestsRandomiser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name;
                        chests.RemoveAt(chestIndex);
                    }
                }
            }

            if (chests.Count <= 0)
                return;

            for (int chest = 0; chest < chests.Count; chest++)
            {
                if (PersistentPrefs.GetInstance().m_currentSaveFile.HasFlag(gameObject.scene.name + "_ChestsRandomiser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name + "_ChestOpened_" + chest))
                    chests[chest].LoadOpen();
                chests[chest].m_lootPickup = m_baseObject;
                chests[chest].m_chestIndex = chest;
                chests[chest].m_flagPrefix = gameObject.scene.name + "_ChestsRandomiser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name;
            }
        }
        else
        {
            for (int thing = 0; thing < m_toFill.Count; thing++)
            {
                for (int i = 0; i < m_toFill[thing].m_amountToPlace; i++)
                {
                    if (chests.Count == 0)
                        return; //No more chest to fill end the start function
                    int chestIndex = Random.Range(0, chests.Count);
                    PersistentPrefs.GetInstance().m_currentSaveFile.SetIntFlag(gameObject.scene.name + "_ChestsRandomiser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name + "_Thing_" + thing + "_" + i, chestIndex);
                    chests[chestIndex].m_lootPickup = m_toFill[thing].m_item;
                    chests[chestIndex].m_chestIndex = chestIndex;
                    chests[chestIndex].m_flagPrefix = gameObject.scene.name + "_ChestsRandomiser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name;
                    chests.RemoveAt(chestIndex);
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
                        PersistentPrefs.GetInstance().m_currentSaveFile.SetIntFlag(gameObject.scene.name + "_ChestsRandomiser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name + "_Mimic_" + i, chestIndex);
                        chests[chestIndex].m_isMimic = true;
                        chests[chestIndex].m_room = m_dungenRoom;
                        chests[chestIndex].m_enemyParent = m_enemyParent;
                        chests[chestIndex].m_chestIndex = chestIndex;
                        chests[chestIndex].m_flagPrefix = gameObject.scene.name + "_ChestsRandomiser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name;
                        chests.RemoveAt(chestIndex);
                    }
                }
            }

            if (chests.Count <= 0)
                return;

            for (int chest = 0; chest < chests.Count; chest++)
            {
                chests[chest].m_lootPickup = m_baseObject;
                chests[chest].m_chestIndex = chest;
                chests[chest].m_flagPrefix = gameObject.scene.name + "_ChestsRandomiser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name;
            }

            PersistentPrefs.GetInstance().m_currentSaveFile.AddFlag(gameObject.scene.name + "_ChestsRandomised_" + gameObject.transform.parent.parent.name + "_" + gameObject.name);

        }

    }

}

[System.Serializable]
public struct ChestRandomThing
{
    public GameObject m_item;
    public int m_amountToPlace;
}