using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Yushan.Enums;
public class UIEquipmentSlot : UIItemSlot
{
    public EquipmentType equipmentType;

    private void OnValidate()
    {
        gameObject.name = "UI Equipment Slot:"+equipmentType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null) return;
        Inventory.instance.UnequipItem(item.data as ItemDataEquipment);
        Inventory.instance.AddItem(item.data as ItemDataEquipment);
        ui.itemTooltip.HideTooltip();
        CleanUpSlot();
    }
}
