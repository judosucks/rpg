using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntityFX : MonoBehaviour
{
   [Header("ailment colors")] 
   [SerializeField] private Color[] chillColor;
   [SerializeField] private Color[] igniteColor;
   [SerializeField] private Color[] shockColor;
   private SpriteRenderer sr;
   [Header("Flash FX")] [SerializeField] private Material hitMat;
   private Material originalMat;

   [Header("player fx")] 
   [SerializeField] private GameObject lightningFx;
   [SerializeField] private float yOffset;
   [SerializeField] private float lightningDuration;
   private GameObject currentLightning;
   
  
   private Keyboard keyboard= Keyboard.current;
   public bool hasDestroyedLightning { get; private set; }= false;

   private void Awake()
   {
      
   }
   
   public void PlayerLightningFx(Transform _playerTransform)
   {
      // Destroy any existing lightning FX before creating a new one.
      if (currentLightning != null)
      {
        
         Destroy(currentLightning);
      }
      Vector3 lightningStartPosition = _playerTransform.position + new Vector3(0, yOffset, 0);
      currentLightning = Instantiate(lightningFx, lightningStartPosition, Quaternion.identity);
      currentLightning.transform.SetParent(_playerTransform);
      
      
      hasDestroyedLightning = false; // Ensure proper state reset
      
      Destroy(currentLightning, lightningDuration);
   }

   public void ForceDestroyLightning()
   {
      Debug.Log("force destroy lightning");
      if (hasDestroyedLightning)
      {
         
         return; // Prevent duplicate execution
      }



      if (currentLightning != null)
      {
         
         Destroy(currentLightning);
         currentLightning = null;
      }

      hasDestroyedLightning = true;
   }

  

   private void Start()
   {
      if (keyboard == null)
      {
         
         keyboard = Keyboard.current;
         return;
      }
      
      sr = GetComponentInChildren<SpriteRenderer>();
      originalMat = sr.material;
   }

   private IEnumerator FlashFX()
   {
      sr.material = hitMat;
      yield return new WaitForSeconds(0.2f);
      sr.material = originalMat;
   }

   
   private void RedColorBlink()
   {
      if (sr.color != Color.white)
      {
         sr.color = Color.white;
      }
      else
      {
         sr.color = Color.red;
      }
   }

   private void CancelColorChange()
   {
      CancelInvoke();
      sr.color = Color.white;
   }
   public void ChillFxFor(float _seconds)
   {
      
      InvokeRepeating("ChillColorFx", 0, .3f);
      Invoke("CancelColorChange", _seconds);
   }


   public void ShockFxFor(float _seconds)
   {
      
      InvokeRepeating("ShockColorFx", 0, .3f);
      Invoke("CancelColorChange", _seconds);
   }
   public void IgnitedFxFor(float _seconds)
   {
      InvokeRepeating("IgniteColorFx", 0, .3f);
      Invoke("CancelColorChange", _seconds);
   }
   private void IgniteColorFx()
   {
      if (sr.color != igniteColor[0])
      {
         sr.color = igniteColor[0];
      }
      else
      {
         sr.color = igniteColor[1];
      }
   }
   private void ChillColorFx()
   {
      if (sr.color != chillColor[0])
         sr.color = chillColor[0];
      else
         sr.color = chillColor[1];
   }

   private void ShockColorFx()
   {
      if (sr.color != shockColor[0])
         sr.color = shockColor[0];
      else
         sr.color = shockColor[1];
   }
   public void MakeTransparent(bool _transparent)
   {
      Color color = sr.color;
      color.a = _transparent ? 0f : 1.0f; // Adjust alpha for transparency
      sr.color = color;
   }
}
