using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Yushan.Enums;
using UnityEngine.InputSystem;
public class UIItemSlot : MonoBehaviour , IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]protected Image itemImage;
    [SerializeField]protected TextMeshProUGUI itemText;
    
    public InventoryItem item;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected UI ui;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
       
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;
        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite =  item.data.icon;
            if(item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }
    public virtual void OnPointerDown(PointerEventData _eventData)
    {
        if(item == null || item.data == null) return;
        if (Keyboard.current.leftCtrlKey.isPressed)
        {
            Debug.Log("drop"+" "+item.data.itemName);
            Inventory.instance.RemoveItem(item.data);
            return;
        }
        if (item.data.itemType == ItemType.Equipment)
        {
            Debug.Log("equip"+" "+item.data.itemName);
            Inventory.instance.EquipItem(item.data);
        }
        ui.itemTooltip.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null || item.data == null) return;

       
        ui.itemTooltip.ShowTooltip(item.data as ItemDataEquipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(item == null || item.data == null) return;
        ui.itemTooltip.HideTooltip();
    }
}
