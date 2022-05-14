using System;
using UnityEngine;

public static class PersistentPrefs
{
    public static string key_saveFileExists = "SaveFileExists";

    public static string key_currentHealth = "PlayerCurrentHealth";

    public static string key_item1Unlocked = "PlayerItem1Unlocked";
    public static string key_item2Unlocked = "PlayerItem2Unlocked";
    public static string key_item3Unlocked = "PlayerItem3Unlocked";
    public static string key_item4Unlocked = "PlayerItem4Unlocked";
    public static string key_item5Unlocked = "PlayerItem5Unlocked";
    public static string key_item6Unlocked = "PlayerItem6Unlocked";
    public static string key_item7Unlocked = "PlayerItem7Unlocked";
    public static string key_item8Unlocked = "PlayerItem8Unlocked";

    public static string key_scene = "PlayerScene";

    // Auto Save is 0
    public static SaveFile m_currentSaveFile = LoadDefaults();

    public static SaveFile LoadDefaults()
    {
        SaveFile saveFile;
        saveFile.currentSave = 0;
        saveFile.currentHealth = 5;
        saveFile.item1Unlocked = true;
        saveFile.item2Unlocked = false;
        saveFile.item3Unlocked = false;
        saveFile.item4Unlocked = false;
        saveFile.item5Unlocked = false;
        saveFile.item6Unlocked = false;
        saveFile.item7Unlocked = false;
        saveFile.item8Unlocked = false;
        saveFile.scene = "Overworld";
        return saveFile;
    }

    public static bool HasSaveFile(int save)
    {
        return (PlayerPrefs.GetInt(key_saveFileExists + save) == 1);
    }

    public static void Load(int save)
    {
        SaveFile saveFile;
        saveFile.currentHealth = PlayerPrefs.GetInt(key_currentHealth + save);
        saveFile.item1Unlocked = PlayerPrefs.GetInt(key_item1Unlocked + save) == 1;
        saveFile.item2Unlocked = PlayerPrefs.GetInt(key_item2Unlocked + save) == 1;
        saveFile.item3Unlocked = PlayerPrefs.GetInt(key_item3Unlocked + save) == 1;
        saveFile.item4Unlocked = PlayerPrefs.GetInt(key_item4Unlocked + save) == 1;
        saveFile.item5Unlocked = PlayerPrefs.GetInt(key_item5Unlocked + save) == 1;
        saveFile.item6Unlocked = PlayerPrefs.GetInt(key_item6Unlocked + save) == 1;
        saveFile.item7Unlocked = PlayerPrefs.GetInt(key_item7Unlocked + save) == 1;
        saveFile.item8Unlocked = PlayerPrefs.GetInt(key_item8Unlocked + save) == 1;
        saveFile.scene = PlayerPrefs.GetString(key_scene);
    }

    public static void Save()
    {
        PlayerPrefs.SetInt(key_currentHealth + m_currentSaveFile.currentSave, m_currentSaveFile.currentHealth);
        PlayerPrefs.SetInt(key_item1Unlocked + m_currentSaveFile.currentSave, m_currentSaveFile.item1Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item2Unlocked + m_currentSaveFile.currentSave, m_currentSaveFile.item2Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item3Unlocked + m_currentSaveFile.currentSave, m_currentSaveFile.item3Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item4Unlocked + m_currentSaveFile.currentSave, m_currentSaveFile.item4Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item5Unlocked + m_currentSaveFile.currentSave, m_currentSaveFile.item5Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item6Unlocked + m_currentSaveFile.currentSave, m_currentSaveFile.item6Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item7Unlocked + m_currentSaveFile.currentSave, m_currentSaveFile.item7Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(key_item8Unlocked + m_currentSaveFile.currentSave, m_currentSaveFile.item8Unlocked ? 1 : 0);
        PlayerPrefs.SetString(key_scene + m_currentSaveFile.currentSave, m_currentSaveFile.scene);
    }
}