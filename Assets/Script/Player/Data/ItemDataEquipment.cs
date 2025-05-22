using UnityEngine;
using System.Collections.Generic;
using Yushan.Enums;
[CreateAssetMenu(fileName = "New Item", menuName = "Item/Equipment")]



public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;
    public float itemCooldown;
    public ItemEffect[] itemEffects;
    [Header("Major stats")] 
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;
    [Header("Offensive stats")] 
    public int damage;
    public int critChance;
    public int critPower;
    [Header("Defensive stats")] 
    public int armor;
    public int health;
    public int evasion;
    public int magicResistance;
    [Header("magic stats")] 
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;
    [Header("craft requirements")] 
    public List<InventoryItem> craftingMaterials;

    private int minDescriptionLength;

    public override string GetDescription()
    {
        sb.Length = 0;
        minDescriptionLength = 0;
        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit.Chance");
        AddItemDescription(critPower, "Crit.Power");

        AddItemDescription(health, "Health");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "Magic.Resist");

        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightningDamage, "Lightning Damage");

        if (minDescriptionLength < 5)
        {
            for (int i = 0; i < itemEffects.Length; i++)
            {
                sb.AppendLine();
                sb.AppendLine("Unique Effect:"+itemEffects[i].effectDescription);
                minDescriptionLength++;
            }
            for (int i = 0; i < 5 - minDescriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
        

    return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (_value > 0)
            {
                sb.Append("+ " +_value + " "+_name);
            }

            minDescriptionLength++;
        }
    }
    public void ItemEffect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExcutedEffect(_enemyPosition);
        }
    }
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);
        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);
        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);
        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);
        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);
        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }
}
