using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponWheelController : MonoBehaviour
{
    public GameObject WeaponWheel;
    public TextMeshProUGUI WeaponSelectedText;
    public float CoolDownCounter;
    public PlayerController pc;
    
    public bool isWheelOpen { get; private set; }
    public Item CurrentItem;

    List<Button> buttons;
    List<WeaponButtonInfo> buttonInfos;
    public int currentIndex = 0;
    public int selectedIndex = 0;

    private void Start()
    {
        buttons = new List<Button>();
        foreach (Transform tr in WeaponWheel.transform)
        {
            buttons.Add(tr.GetComponent<Button>());
        }

        buttonInfos = new List<WeaponButtonInfo>();
        foreach (Transform tr in WeaponWheel.transform)
        {
            buttonInfos.Add(tr.GetComponent<WeaponButtonInfo>());
        }
    }

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
            WeaponSelectedText.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            EventSystem.current.SetSelectedGameObject(null);
            buttons[selectedIndex].Select();
        }
        else
        {
            WeaponWheel.SetActive(false);
            WeaponSelectedText.gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Pulse the weapon wheel with an angle.
    public void Pulse(float angle)
    {
        int count = WeaponWheel.transform.childCount;
        angle += Mathf.PI / 2;
        angle -= (Mathf.PI * 2) / (count * 2);
        angle %= Mathf.PI * 2;
        angle /= Mathf.PI * 2;
        angle *= count;
        int item = (count - 1) - (int)(angle % count);
        //Debug.Log(item);

        if (currentIndex != item)
        {
            buttons[currentIndex].OnPointerExit(null);
            buttonInfos[currentIndex].OnPointerExit(null);
            buttons[item].OnPointerEnter(null);
            buttonInfos[item].OnPointerEnter(null);
            currentIndex = item;
        }
    }

    public void UpdateText(WeaponButtonInfo weaponScript)
    {
        Item itemSelected = weaponScript.WheelItem;
        WeaponSelectedText.text = itemSelected.name;
    }

    public void SelectItem(WeaponButtonInfo weaponScript)
    {
        Item itemSelected = weaponScript.WheelItem;
        if (itemSelected == CurrentItem)
        {
            Debug.Log("Item Already Selected: " + itemSelected.name);
        }
        else
        {
            CurrentItem = itemSelected;
            Debug.Log("Item Selected: " + itemSelected.name);
        }
        ToggleWheel();
    }

    public void SelectItem(int index)
    {
        buttons[selectedIndex].OnDeselect(null);
        buttons[index].Select();
        buttons[index].OnSelect(null);
        buttons[index].onClick.Invoke();
        selectedIndex = index;
    }
}
