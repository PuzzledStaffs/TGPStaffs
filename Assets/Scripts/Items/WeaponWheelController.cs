using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class WeaponWheelController : MonoBehaviour
{
    [FormerlySerializedAs("WeaponWheel")]
    public GameObject m_weaponWheel;
    [FormerlySerializedAs("WeaponSelectedText")]
    public TextMeshProUGUI m_weaponSelectedText;
    [FormerlySerializedAs("CoolDownCounter")]
    public float m_CoolDownCounter;
    [FormerlySerializedAs("pc")]
    public PlayerController m_pc;
    public bool isWheelOpen { get; private set; }

    [FormerlySerializedAs("CurrentItem")]
    public Item m_CurrentItem;
    [FormerlySerializedAs("buttons")]
    List<Button> m_buttonsList;
    [FormerlySerializedAs("buttonInfos")]
    List<WeaponButtonInfo> m_buttonInfos;
    [FormerlySerializedAs("currentIndex")]
    public int m_currentIndex = 0;
    [FormerlySerializedAs("selectedIndex")]
    public int m_selectedIndex = 0;

    [FormerlySerializedAs("Buttons")]
    public WeaponButtonInfo[] m_buttons;
    [FormerlySerializedAs("ItemUnlockedUI")]
    public ItemUnlockedUI m_itemUnlockedUI;
    [FormerlySerializedAs("ItemSelectedIcon")]
    public Image m_itemSelectedIcon;




    private void Start()
    {
        m_buttonsList = new List<Button>();
        foreach (Transform tr in m_weaponWheel.transform)
        {
            m_buttonsList.Add(tr.GetComponent<Button>());
        }

        m_buttonInfos = new List<WeaponButtonInfo>();
        foreach (Transform tr in m_weaponWheel.transform)
        {
            m_buttonInfos.Add(tr.GetComponent<WeaponButtonInfo>());
        }
    }

    public void LeftClickAction()
    {
        if (m_CoolDownCounter <= 0)
        {
          m_CoolDownCounter = m_CurrentItem.MaxCooldown;
          m_CurrentItem.LeftClickAction(m_pc);
        }
    }

    public void LeftClickHoldAction()
    {
        m_CurrentItem.LeftClickAction(m_pc);
    }

    public void HoldActionCooldown()
    {
        m_CurrentItem.ReleaseAction(m_pc);
        m_CoolDownCounter = m_CurrentItem.MaxCooldown;
    }


    private void Update()
    {
        m_itemSelectedIcon.sprite = m_CurrentItem.ItemIcon;

        if (m_pc.m_buttonHeld)
        {
            LeftClickHoldAction();
        }

        if (m_CoolDownCounter >= 0)
        {
            m_CoolDownCounter -= Time.deltaTime;
        }
    }

    public void ToggleWheel()
    {
        isWheelOpen = !isWheelOpen;
        if (isWheelOpen)
        {
            m_weaponWheel.SetActive(true);
            m_weaponSelectedText.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            EventSystem.current.SetSelectedGameObject(null);
            m_buttonsList[m_selectedIndex].Select();
        }
        else
        {
            m_weaponWheel.SetActive(false);
            m_weaponSelectedText.gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Pulse the weapon wheel with an angle.
    public void Pulse(float angle)
    {
        int count = m_weaponWheel.transform.childCount;
        angle += Mathf.PI / 2;
        angle -= (Mathf.PI * 2) / (count * 2);
        angle %= Mathf.PI * 2;
        angle /= Mathf.PI * 2;
        angle *= count;
        int item = (count - 1) - (int)(angle % count);
        //Debug.Log(item);

        if (m_currentIndex != item)
        {
            m_buttons[m_currentIndex].OnPointerExit(null);
            m_buttonInfos[m_currentIndex].OnPointerExit(null);
            m_buttons[item].OnPointerEnter(null);
            m_buttonInfos[item].OnPointerEnter(null);
            m_currentIndex = item;
        }
    }

    public void UpdateText(WeaponButtonInfo weaponScript)
    {
        Item itemSelected = weaponScript.WheelItem;
        m_weaponSelectedText.text = itemSelected.name;
    }

    public void SelectItem(WeaponButtonInfo weaponScript)
    {
        if (!weaponScript.ItemBlocked)
        {
            Item itemSelected = weaponScript.WheelItem;
            if (itemSelected == m_CurrentItem)
            {
                Debug.Log("Item Already Selected: " + itemSelected.name);
            }
            else
            {
                m_CurrentItem = itemSelected;
                Debug.Log("Item Selected: " + itemSelected.name);
            }
            ToggleWheel();
        }
    }

    public void SelectItem(int index)
    {
        m_buttonsList[m_selectedIndex].OnDeselect(null);
        m_buttonsList[index].Select();
        m_buttonsList[index].OnSelect(null);
        m_buttonsList[index].onClick.Invoke();
        m_selectedIndex = index;
    }
}
