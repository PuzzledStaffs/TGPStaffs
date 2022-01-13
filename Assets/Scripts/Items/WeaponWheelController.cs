using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponWheelController : MonoBehaviour
{
    public GameObject WeaponWheel;
    public TextMeshProUGUI WeaponSelectedText;
    public bool isWheelOpen { get; private set; }

    public void ToggleWheel()
    {
        isWheelOpen = !isWheelOpen;
        if (isWheelOpen)
        {
            WeaponWheel.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            WeaponWheel.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Pulse the weapon wheel with an angle.
    public void Pulse(float angle)
    {

    }

    public void UpdateText(WeaponButtonInfo weaponScript)
    {
        Item itemSelected = weaponScript.WheelItem;
        WeaponSelectedText.text = itemSelected.name;
    }

    public void SelectItem(WeaponButtonInfo weaponScript)
    {
        Item itemSelected = weaponScript.WheelItem;
        Debug.Log("Item Selected: " + itemSelected.name);
    }
}
