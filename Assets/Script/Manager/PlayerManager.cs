using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class PlayerManager : MonoBehaviour
// {
//    public static PlayerManager instance;
//
//    public Player player;
//    private void Awake()
//    {
//       if (instance != null)
//       {
//          Destroy(instance.gameObject);
//       }
//       else
//       {
//          instance = this;
//       }
//    }
// }
public class PlayerManager : MonoBehaviour,ISaveManager
{
   public static PlayerManager instance;
   public Player player;
   public int currency;
   public PlayerData playerData;
   private void Awake()
   {
      Debug.Log("who calls first awake from playermanager");
      // 确保 PlayerManager 在场景中的唯一性
      
      if (instance != null)
      {
         Destroy(instance.gameObject);
      }
      else
      {
         instance = this;
      }
      
      // 确保 player 已经赋值
      if (player == null)
      {
         player = FindFirstObjectByType<Player>(); // 或者使用您特定的init逻辑
      }

      playerData = player.playerData;
   }

   public bool HaveEnoughExperience(int _experience)
   {
      if (_experience > currency)
      {
         Debug.Log("not enough experience");
         return false;
      }
      currency = currency - _experience;
      return true;
   }

   public int CurrentCurrencyAmount() => currency;
   public void LoadData(GameData _data)
   {
      this.currency = _data.currency;
   }

   public void SaveData(ref GameData _data)
   {
      _data.currency = this.currency;
   }
}