using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WeaponButtonInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item WheelItem;
    public Image WheelIcon;
    [SerializeField] bool ItemBlocked = false;
    public Sprite LockIcon;
    public GameObject SelectionImage;
    public WeaponWheelController weaponController;


    //Button Lerp Scalling
    RectTransform rect;
    Vector2 originalScale;
    Vector2 desiredScale;
    public float ScaleFactor = 1.20f;

    private void Start()
    {
        //Assign the item icon to the weapon wheel button
        WheelIcon.sprite = WheelItem.ItemIcon;

        //Set up the scale lerp values
        rect = gameObject.GetComponent<RectTransform>();
        originalScale = rect.localScale;
        desiredScale = new Vector2(1, 1);
        desiredScale = new Vector2(originalScale.x, originalScale.y);
    }

    public void Update()
    {
        //make the button interactable bool be represented by the itemblocked boolean
        GetComponent<Button>().interactable = !ItemBlocked;
        //update the lacal scale of the buttons
        rect.localScale = Vector2.Lerp(rect.localScale, desiredScale, Time.deltaTime * 5);

        //IF the button is blocked change the icon
        if (ItemBlocked)
        {
            WheelIcon.sprite = LockIcon;
        }
        else
        {
            WheelIcon.sprite = WheelItem.ItemIcon;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (!ItemBlocked)
        {
            SelectionImage.SetActive(true);
            desiredScale = new Vector2(originalScale.x * ScaleFactor, originalScale.y * ScaleFactor);
            weaponController.UpdateText(this);
        }
    }


    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (!ItemBlocked)
        {
            SelectionImage.SetActive(false);
            desiredScale = new Vector2(originalScale.x, originalScale.y);

        }
    }
}


