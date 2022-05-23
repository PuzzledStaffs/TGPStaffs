using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public bool m_saveLoaded = false;

    public string m_saveDate;
    public int m_saveSeconds;
    public int m_saveMinutes;
    public int m_saveHours;

    public int m_currentHealth;
    public string m_currentScene;
    public bool m_isInDungeon;

    // Vector3 is not serilizable so must store seperately
    public float m_savePositionX;
    public float m_savePositionY;
    public float m_savePositionZ;

    public bool m_item1Unlocked;
    public bool m_item2Unlocked;
    public bool m_item3Unlocked;
    public bool m_item4Unlocked;
    public bool m_item5Unlocked;
    public bool m_item6Unlocked;
    public bool m_item7Unlocked;
    public bool m_item8Unlocked;

    public List<string> m_flags;

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
    }

    public void AddSecond()
    {
        m_saveSeconds++;

        if (m_saveSeconds >= 60)
        {
            m_saveMinutes += m_saveSeconds / 60;
            m_saveSeconds %= 60;
        }

        if (m_saveMinutes >= 60)
        {
            m_saveHours += m_saveMinutes / 60;
            m_saveMinutes %= 60;
        }
    }

    public bool HasFlag(string key)
    {
        return m_flags.Contains(key);
    }

    public void AddFlag(string key)
    {
        if (!HasFlag(key))
            m_flags.Add(key);
    }

    public void RemoveFlag(string key)
    {
        if (HasFlag(key))
            m_flags.Remove(key);
    }
}
