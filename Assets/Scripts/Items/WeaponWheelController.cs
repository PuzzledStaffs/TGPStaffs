using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponWheelController : MonoBehaviour
{
    public GameObject WeaponWheel;
    public TextMeshProUGUI WeaponSelectedText;
    public float CoolDownCounter;
    public PlayerController pc;
    
    public bool isWheelOpen { get; private set; }
    public Item CurrentItem;


    public void LeftClickAction()
    {
        if (CoolDownCounter <= 0)
        {
          CoolDownCounter = CurrentItem.MaxCooldown;
          CurrentItem.LeftClickAction(pc);
        }
    }

    public void LeftClickHoldAction()
    {
        CurrentItem.LeftClickAction(pc);
    }

    public void HoldActionCooldown()
    {
        CurrentItem.ReleaseAction(pc);
        CoolDownCounter = CurrentItem.MaxCooldown;
    }


    private void Update()
    {
        if (pc.ButtonHeld)
        {
            LeftClickHoldAction();
        }

        if (CoolDownCounter >= 0)
        {
            CoolDownCounter -= Time.deltaTime;
        }
    }


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
        angle /= Mathf.PI;
        angle *= WeaponWheel.transform.childCount;
        int item = (int)(angle % WeaponWheel.transform.childCount);
        Debug.Log(item);
    }

    public void UpdateText(WeaponButtonInfo weaponScript)
    {
        Item itemSelected = weaponScript.WheelItem;
        WeaponSelectedText.text = itemSelected.name;
    }

    public void SelectItem(WeaponButtonInfo weaponScript)
    {
        Item itemSelected = weaponScript.WheelItem;
        CurrentItem = itemSelected;
        ToggleWheel();
        Debug.Log("Item Selected: " + itemSelected.name);
    }
}
