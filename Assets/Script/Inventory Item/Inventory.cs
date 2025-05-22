using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using Yushan.Enums;


public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;
    public List<ItemData> defaultItems;
    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;
    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;

    [Header("inventory ui")] [SerializeField]
    private Transform inventorySlotParent;

    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;
    private UIItemSlot[] inventoryItemSlot;
    private UIItemSlot[] stashItemSlot;
    private UIEquipmentSlot[] equipmentItemSlot;
    private UIStatSlot[] statSlot;

    [Header("Item cooldown")] private float lastTimeUsedFlask;

    private float lastTimeUsedArmor;
    public float flaskCooldown { get; private set; }
    public float armorCooldown { get; private set; }
    [Header("data base")]
    
    public List<InventoryItem> loadedItem;
    public List<ItemDataEquipment> loadedEquipment;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UIItemSlot>();
        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();
        equipmentItemSlot = equipmentSlotParent.GetComponentsInChildren<UIEquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UIStatSlot>();
        DefaultItems();
    }

    private void DefaultItems()
    {
        Debug.LogWarning("default items");
        foreach (ItemDataEquipment item in loadedEquipment)
        {
            Debug.LogWarning("equip item" + item.name);
            EquipItem(item);
        }
        
        if (loadedItem.Count > 0)
        {
            Debug.LogWarning("default items count" + defaultItems.Count);
            foreach (InventoryItem item in loadedItem)
            {
                Debug.LogWarning("default item" + item.data.name);
                for(int i = 0; i < item.stackSize; i++)
                {
                    Debug.LogWarning("default item stack size" + item.stackSize);
                        AddItem(item.data);
                    
                }
            }
            return;
        }
        for (int i = 0; i < defaultItems.Count; i++)
        {
            Debug.LogWarning("default item" + defaultItems[i].name);
            if (defaultItems[i] != null)
            {
                Debug.LogWarning("default item not null" + defaultItems[i].name);
                AddItem(defaultItems[i]);
            }
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;
        if (newEquipment == null)
        {
            Debug.Log("not an equipment");
            return;
        }
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipment oldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemDataEquipment _itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(_itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(_itemToRemove);
            _itemToRemove.RemoveModifiers();

        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentItemSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentItemSlot[i].equipmentType)
                {
                    equipmentItemSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatSlotUI();
    }

    public void UpdateStatSlotUI()
    {
        for (int i = 0; i < statSlot.Length; i++) //update info of stat slot
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public bool CanAddItem()
    {
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.Log("inventory full");
            return false;
        }

        return true;
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddItemToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddItemToStash(_item);
        }

        UpdateSlotUI();
    }

    private void AddItemToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddItemToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }

        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI();
    }

    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame && inventory.Count > 0)
        {
            ItemData newItem = inventory[inventory.Count - 1].data;
            RemoveItem(newItem);
        }
    }

    public bool CanCraft(ItemDataEquipment _itemToCraft, List<InventoryItem> _requireMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        for (int i = 0; i < _requireMaterials.Count; i++)
        {
            if (!stashDictionary.TryGetValue(_requireMaterials[i].data, out InventoryItem stashValue) ||
                stashValue.stackSize < _requireMaterials[i].stackSize)
            {
                Debug.Log("not enough materials");
                return false;
            }
            else
            {
                materialsToRemove.Add(stashValue);
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            if (materialsToRemove[i].data != null)
            {
                RemoveItem(materialsToRemove[i].data);
            }
        }

        AddItem(_itemToCraft);
        Debug.Log("here is your item" + _itemToCraft.name);
        return true;
    }

    public List<InventoryItem> GetEquipment() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemDataEquipment GetEquipmentByType(EquipmentType _type)
    {
        ItemDataEquipment equipedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equipedItem = item.Key;
            }
        }

        return equipedItem;
    }

    public void UseFlask()
    {
        ItemDataEquipment currentFlask = GetEquipmentByType(EquipmentType.Flask);
        if (currentFlask == null)
        {
            Debug.Log("no more flask");
            return;
        }

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;
        if (canUseFlask)
        {
            Debug.Log("can use flask");
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.ItemEffect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("cant use flask");
        }
    }

    public bool CanUseArmor()
    {
        ItemDataEquipment currentArmor = GetEquipmentByType(EquipmentType.Armor);
        if (Time.time > lastTimeUsedArmor + armorCooldown)
        {
            Debug.Log("can use armor");
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        Debug.Log("cant use armor");
        return false;
    }

    public void LoadData(GameData _data)
    {
        Debug.Log("items loaded");
        foreach (KeyValuePair<string,int> pair in _data.inventory)
        {
            Debug.LogWarning("loaded item id" + pair.Key);
            foreach (var item in GetItemDataBase())
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;
                    
                    loadedItem.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemId in _data.equipmentId)
        {
            Debug.LogWarning("loaded item id" + loadedItemId);
            foreach (var item in GetItemDataBase())
            {
                if(item != null && loadedItemId == item.itemId)
                {
                    loadedEquipment.Add(item as ItemDataEquipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();
        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }
    }

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData>itemDataBase = new List<ItemData>();
        #if UNITY_EDITOR
        string[] assetNames = AssetDatabase.FindAssets
            ("", new[] { "Assets/Script/Player/Data/Item" });
        
        foreach (string SOName in assetNames)
        {
            var SOpth = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpth);
            Debug.Log("SOPth"+SOpth+"itemdada"+itemData);
            itemDataBase.Add(itemData);
        }
        #endif
        return itemDataBase;
    }
}


