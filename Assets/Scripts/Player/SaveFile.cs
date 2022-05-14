using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SaveFile
{
    public int m_currentHealth;
    public string m_currentScene;

    public bool m_item1Unlocked;
    public bool m_item2Unlocked;
    public bool m_item3Unlocked;
    public bool m_item4Unlocked;
    public bool m_item5Unlocked;
    public bool m_item6Unlocked;
    public bool m_item7Unlocked;
    public bool m_item8Unlocked;

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
        PersistentPrefs.GetInstance().SaveSaveFile(0);
    }
}
