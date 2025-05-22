using System;
using UnityEngine;
using Yushan.Enums;
using TMPro;
using UnityEngine.EventSystems;

public class UIStatSlot : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler
{
    private UI ui;
    [SerializeField]private string statName;
    [SerializeField]private StatType statType;
    [SerializeField]private TextMeshProUGUI statValue;
    [SerializeField]private TextMeshProUGUI statNameText;
    [TextArea]
    [SerializeField]private string statDescription;
    private void OnValidate()
    {
        gameObject.name = "stat -" + statName;
        if (statNameText != null)
        {
            statNameText.text = statName;
        }
    }

    private void Start()
    {
        UpdateStatValueUI();
        ui = GetComponentInParent<UI>();
    }

    
    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            statValue.text = playerStats.StatToModify(statType).GetValue().ToString();

            if (statType == StatType.health)
                statValue.text = playerStats.GetMaxHealthValue().ToString();
            if (statType == StatType.damage)
                statValue.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
            if(statType == StatType.critPower)
                statValue.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
            if(statType == StatType.critChance)
                statValue.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();
            if(statType == StatType.evasion)
                statValue.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();
            if (statType == StatType.magicRes)
                statValue.text = (playerStats.magicResistance.GetValue() + (playerStats.intelligence.GetValue() * 3))
                    .ToString();


        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltip.ShowStatTooltip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       ui.statTooltip.HideStatTooltip();
    }
}
