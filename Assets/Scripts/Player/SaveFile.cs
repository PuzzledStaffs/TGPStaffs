using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct SaveFile
{
    public int currentSave;

    public int currentHealth;

    public bool item1Unlocked;
    public bool item2Unlocked;
    public bool item3Unlocked;
    public bool item4Unlocked;
    public bool item5Unlocked;
    public bool item6Unlocked;
    public bool item7Unlocked;
    public bool item8Unlocked;

    public string scene;

    public void UnlockItem(int i)
    {
        switch (i)
        {
            case 1:
                item1Unlocked = true;
                break;
            case 2:
                item2Unlocked = true;
                break;
            case 3:
                item3Unlocked = true;
                break;
            case 4:
                item4Unlocked = true;
                break;
            case 5:
                item5Unlocked = true;
                break;
            case 6:
                item6Unlocked = true;
                break;
            case 7:
                item7Unlocked = true;
                break;
            case 8:
                item8Unlocked = true;
                break;
            default:
                break;
        }
    }
}
