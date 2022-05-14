using System;
using UnityEngine;

public class PersistentPrefs
{
    #region Save File Keys
    public static string KEY_SAVE_FILE_EXISTS = "SaveFileExists";

    public static string KEY_CURRENT_HEALTH = "PlayerCurrentHealth";

    public static string KEY_ITEM_1_UNLOCKED = "PlayerItem1Unlocked";
    public static string KEY_ITEM_2_UNLOCKED = "PlayerItem2Unlocked";
    public static string KEY_ITEM_3_UNLOCKED = "PlayerItem3Unlocked";
    public static string KEY_ITEM_4_UNLOCKED = "PlayerItem4Unlocked";
    public static string KEY_ITEM_5_UNLOCKED = "PlayerItem5Unlocked";
    public static string KEY_ITEM_6_UNLOCKED = "PlayerItem6Unlocked";
    public static string KEY_ITEM_7_UNLOCKED = "PlayerItem7Unlocked";
    public static string KEY_ITEM_8_UNLOCKED = "PlayerItem8Unlocked";

    public static string key_scene = "PlayerScene";
    #endregion

    #region Settings Keys
    public static string KEY_SETTINGS_EXISTS = "SettingsExist";
    public static string KEY_SETTINGS_VOLUME = "SettingsVolume";
    #endregion

    public SaveFile m_currentSaveFile;
    public Settings m_settings;

    private static PersistentPrefs _instance = new PersistentPrefs();

    public PersistentPrefs()
    {
        LoadDefaultSave();
        if (PlayerPrefs.GetInt(KEY_SETTINGS_EXISTS) == 1)
            LoadSettings();
        else
            LoadDefaultSettings();
    }

    public static PersistentPrefs GetInstance()
    {
        if (_instance == null)
            _instance = new PersistentPrefs();
        return _instance;
    }

    #region Save File Methods
    public void LoadDefaultSave()
    {
        m_currentSaveFile = new SaveFile
        {
            m_currentHealth = 5,
            m_item1Unlocked = true,
            m_item2Unlocked = false,
            m_item3Unlocked = false,
            m_item4Unlocked = false,
            m_item5Unlocked = false,
            m_item6Unlocked = false,
            m_item7Unlocked = false,
            m_item8Unlocked = false,
            m_currentScene = "Overworld"
        };
    }

    public bool HasSaveFile(int save)
    {
        return (PlayerPrefs.GetInt(KEY_SAVE_FILE_EXISTS + save) == 1);
    }

    public void LoadSaveFile(int save)
    {
        SaveFile saveFile = new SaveFile
        {
            m_currentHealth = PlayerPrefs.GetInt(KEY_CURRENT_HEALTH + save),
            m_item1Unlocked = PlayerPrefs.GetInt(KEY_ITEM_1_UNLOCKED + save) == 1,
            m_item2Unlocked = PlayerPrefs.GetInt(KEY_ITEM_2_UNLOCKED + save) == 1,
            m_item3Unlocked = PlayerPrefs.GetInt(KEY_ITEM_3_UNLOCKED + save) == 1,
            m_item4Unlocked = PlayerPrefs.GetInt(KEY_ITEM_4_UNLOCKED + save) == 1,
            m_item5Unlocked = PlayerPrefs.GetInt(KEY_ITEM_5_UNLOCKED + save) == 1,
            m_item6Unlocked = PlayerPrefs.GetInt(KEY_ITEM_6_UNLOCKED + save) == 1,
            m_item7Unlocked = PlayerPrefs.GetInt(KEY_ITEM_7_UNLOCKED + save) == 1,
            m_item8Unlocked = PlayerPrefs.GetInt(KEY_ITEM_8_UNLOCKED + save) == 1,
            m_currentScene = PlayerPrefs.GetString(key_scene)
        };
    }

    public void SaveSaveFile(int save)
    {
        PlayerPrefs.SetInt(KEY_CURRENT_HEALTH + save, m_currentSaveFile.m_currentHealth);
        PlayerPrefs.SetInt(KEY_ITEM_1_UNLOCKED + save, m_currentSaveFile.m_item1Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(KEY_ITEM_2_UNLOCKED + save, m_currentSaveFile.m_item2Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(KEY_ITEM_3_UNLOCKED + save, m_currentSaveFile.m_item3Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(KEY_ITEM_4_UNLOCKED + save, m_currentSaveFile.m_item4Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(KEY_ITEM_5_UNLOCKED + save, m_currentSaveFile.m_item5Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(KEY_ITEM_6_UNLOCKED + save, m_currentSaveFile.m_item6Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(KEY_ITEM_7_UNLOCKED + save, m_currentSaveFile.m_item7Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(KEY_ITEM_8_UNLOCKED + save, m_currentSaveFile.m_item8Unlocked ? 1 : 0);
        PlayerPrefs.SetString(key_scene + save, m_currentSaveFile.m_currentScene);
        PlayerPrefs.SetInt(KEY_SAVE_FILE_EXISTS + save, 1);
    }
    #endregion

    #region Settings Methods
    public void LoadDefaultSettings()
    {
        m_settings = new Settings
        {
            m_volume = 1.0f
        };
    }

    public void LoadSettings()
    {
        m_settings = new Settings
        {
            m_volume = PlayerPrefs.GetFloat(KEY_SETTINGS_VOLUME)
        };
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt(KEY_SETTINGS_EXISTS, 1);
        PlayerPrefs.SetFloat(KEY_SETTINGS_VOLUME, m_settings.m_volume);
    }
    #endregion
}