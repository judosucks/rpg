using UnityEngine;
using System;

[Serializable]
public class InventoryItem
{
   public ItemData data;
   public int stackSize;

   public InventoryItem(ItemData _newData)
   {
      data = _newData;
      AddStack();
   }
   
   public void AddStack()=> stackSize++;
   public void RemoveStack()=> stackSize--;
}
