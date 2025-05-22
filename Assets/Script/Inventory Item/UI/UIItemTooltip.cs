using System;
using UnityEngine;
using TMPro;
using UnityEditor;

public class UIItemTooltip : UITooltip
{
    public RectTransform rect;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private int defaultFontSize = 32;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    

    public void ShowTooltip(ItemDataEquipment item)
    {
        if (item == null) return;
        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();
        AdjustTooltiposition();
        AdjustFontSize(itemNameText);
        
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
