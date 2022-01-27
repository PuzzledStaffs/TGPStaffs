using System;
using UnityEngine;

public class PersistentPrefs : MonoBehaviour
{
    private int currentSave = 0;

    public static string key_currentHealth = "PlayerCurrentHealth";
    public int m_currentHealth;

    public static string key_item1Unlocked = "PlayerItem1Unlocked";
    public bool m_item1Unlocked;

    public static string key_item2Unlocked = "PlayerItem2Unlocked";
    public bool m_item2Unlocked;

    public static string key_item3Unlocked = "PlayerItem3Unlocked";
    public bool m_item3Unlocked;

    public static string key_item4Unlocked = "PlayerItem4Unlocked";
    public bool m_item4Unlocked;

    public static string key_item5Unlocked = "PlayerItem5Unlocked";
    public bool m_item5Unlocked;

    public static string key_item6Unlocked = "PlayerItem6Unlocked";

    public bool m_item6Unlocked;

    public static string key_item7Unlocked = "PlayerItem7Unlocked";
    public bool m_item7Unlocked;

    public static string key_item8Unlocked = "PlayerItem8Unlocked";
    public bool m_item8Unlocked;

    public static string key_scene = "PlayerScene";
    public string m_scene;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadDefaults()
    {
        m_currentHealth = 5;
        m_item1Unlocked = true;
        m_item2Unlocked = false;
        m_item3Unlocked = false;
        m_item4Unlocked = false;
        m_item5Unlocked = false;
        m_item6Unlocked = false;
        m_item7Unlocked = false;
        m_item8Unlocked = false;
        m_scene = "Overworld";
    }

    public void Load(int save)
    {
        m_currentHealth = PlayerPrefs.GetInt(key_currentHealth + save);
        m_item1Unlocked = PlayerPrefs.GetInt(key_item1Unlocked + save) == 1;
        m_item2Unlocked = PlayerPrefs.GetInt(key_item2Unlocked + save) == 1;
        m_item3Unlocked = PlayerPrefs.GetInt(key_item3Unlocked + save) == 1;
        m_item4Unlocked = PlayerPrefs.GetInt(key_item4Unlocked + save) == 1;
        m_item5Unlocked = PlayerPrefs.GetInt(key_item5Unlocked + save) == 1;
        m_item6Unlocked = PlayerPrefs.GetInt(key_item6Unlocked + save) == 1;
        m_item7Unlocked = PlayerPrefs.GetInt(key_item7Unlocked + save) == 1;
        m_item8Unlocked = PlayerPrefs.GetInt(key_item8Unlocked + save) == 1;
        m_scene = PlayerPrefs.GetString(key_scene);
    }

    public void Save(int save)
    {
        PlayerPrefs.SetInt(key_currentHealth + save, m_currentHealth);
        PlayerPrefs.SetInt(key_item1Unlocked + save, m_item1Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item2Unlocked + save, m_item2Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item3Unlocked + save, m_item3Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item4Unlocked + save, m_item4Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item5Unlocked + save, m_item5Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item6Unlocked + save, m_item6Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item7Unlocked + save, m_item7Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item8Unlocked + save, m_item8Unlocked ? 1 : 0);
        PlayerPrefs.SetString(key_scene + save, m_scene);
    }

    /*private void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key + currentSave, value ? 1 : 0);
    }

    private void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key + currentSave, value);
    }

    private void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key + currentSave, value);
    }*/

    public void UnlockItem(int i)
    {
        switch (i)
        {
            case 1:
                m_item1Unlocked = true;
                break;
            case 2:
                m_item2Unlocked = true;
                break;
            case 3:
                m_item3Unlocked = true;
                break;
            case 4:
                m_item4Unlocked = true;
                break;
            case 5:
                m_item5Unlocked = true;
                break;
            case 6:
                m_item6Unlocked = true;
                break;
            case 7:
                m_item7Unlocked = true;
                break;
            case 8:
                m_item8Unlocked = true;
                break;
            default:
                break;
        }
        Save(currentSave);
    }
}