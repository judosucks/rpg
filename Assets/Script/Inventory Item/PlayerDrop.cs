using UnityEngine;
using System.Collections.Generic;
public class PlayerDrop : ItemDrop
{
   [Header("player drop")] 
   [SerializeField] private float chanceToDrop;
   [SerializeField] private float chanceToDropStash;

   public override void GenerateDrop()
   {
      Inventory inventory = Inventory.instance;
      List<InventoryItem> currentStash = inventory.GetStashList();
      List<InventoryItem> currentEquipment = inventory.GetEquipment();
      List<InventoryItem> itemsToUnEquip = new List<InventoryItem>();
      List<InventoryItem> stashToLoose = new List<InventoryItem>();
      foreach (InventoryItem item in currentEquipment)
      {
         if (Random.Range(0, 100) <= chanceToDrop)
         {
            DropItem(item.data);
            itemsToUnEquip.Add(item);
         }
      }

      for (int i = 0; i < itemsToUnEquip.Count; i++)
      {
         inventory.UnequipItem(itemsToUnEquip[i].data as ItemDataEquipment);
      }

      foreach (InventoryItem item in currentStash)
      {
         if (Random.Range(0, 100) <= chanceToDropStash)
         {
            DropItem(item.data);
            stashToLoose.Add(item);;
         }   
      }

      for (int i = 0; i < stashToLoose.Count; i++)
      {
         inventory.RemoveItem(stashToLoose[i].data);
      }
      
   }
}
