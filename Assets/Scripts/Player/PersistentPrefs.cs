using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PersistentPrefs
{
    #region Settings Keys
    public static string KEY_SETTINGS_EXISTS = "SettingsExist";
    public static string KEY_SETTINGS_VOLUME = "SettingsVolume";
    #endregion

    public SaveFile m_currentSaveFile;
    public float m_volume;

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
            m_saveDate = DateTime.Now.ToString("dd/MM/yyyy"),
            m_saveSeconds = 0,
            m_saveMinutes = 0,
            m_saveHours = 0,
            m_currentHealth = 5,
            m_item1Unlocked = true,
            m_item2Unlocked = false,
            m_item3Unlocked = false,
            m_item4Unlocked = false,
            m_item5Unlocked = false,
            m_item6Unlocked = false,
            m_item7Unlocked = false,
            m_item8Unlocked = false,
            m_currentScene = "Overworld",
            m_isInDungeon = false,
            m_savePositionX = 0.0f,
            m_savePositionY = 0.0f,
            m_savePositionZ = 0.0f,
            m_saveLoaded = false,
            m_flags = new System.Collections.Generic.Dictionary<string, bool>()
        };
    }

    public bool HasSaveFile(int save)
    {
        return File.Exists(Application.persistentDataPath + "/SaveFile" + save + ".save");
    }

    public SaveFile LoadSaveFile(int save)
    {
        if (File.Exists(Application.persistentDataPath + "/SaveFile" + save + ".save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveFile" + save + ".save", FileMode.Open);
            SaveFile saveFile = (SaveFile)bf.Deserialize(file);
            file.Close();

            return saveFile;
        }
        return null;
    }

    public void SaveSaveFile(int save)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/SaveFile" + save + ".save");
        bf.Serialize(file, m_currentSaveFile);
        file.Close();
    }
    #endregion

    #region Settings Methods
    public void LoadDefaultSettings()
    {
        m_volume = 1.0f;
    }

    public void LoadSettings()
    {
        m_volume = PlayerPrefs.GetFloat(KEY_SETTINGS_VOLUME);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt(KEY_SETTINGS_EXISTS, 1);
        PlayerPrefs.SetFloat(KEY_SETTINGS_VOLUME, m_volume);
    }
    #endregion
}