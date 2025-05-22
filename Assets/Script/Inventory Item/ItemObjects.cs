using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Yushan.Enums;

public class ItemObjects : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField]private ItemData itemData;
    

    private void SetupVisual()
    {
        if(itemData == null) return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Objects:"+itemData.itemName;
    }
    

    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.linearVelocity = _velocity;
        SetupVisual();
    }

    public void PickUpItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.linearVelocity = new Vector2(0, 7);
            return;
        }
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
