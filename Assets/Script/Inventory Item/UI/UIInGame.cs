using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;
using Yushan.Enums;

public class UIInGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image grenadeImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;
    [SerializeField] private Slider slider;
    public Toggle optionMenuToggle;
    private Keyboard keybord;
    private Mouse mouse;
    private SkillManager skills;
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulAmount;
    [SerializeField] private float increaseRate = 100;
    

    [SerializeField] private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        keybord = Keyboard.current;
        mouse = Mouse.current;
        
    }

    void Start()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged += UpdateHealthUI;
        }

        skills = SkillManager.instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSoulUI();

        if (keybord != null && mouse != null)
        {
            if (keybord.fKey.wasPressedThisFrame && skills.dashSkill.dashUnlocked)
            {
              SetCooldownOf(dashImage);
            }

            if (keybord.rKey.wasPressedThisFrame && skills.blackholeSkill.unlockBlackhole)
            {
                SetCooldownOf(blackholeImage);
            }

            if (keybord.qKey.wasPressedThisFrame && skills.parrySkill.parryUnlocked)
            {
                SetCooldownOf(parryImage);
            }

           
              if (mouse.rightButton.wasReleasedThisFrame && skills.grenadeSkill.grenadeUnlocked)
              {
                 if (mouse.rightButton.wasReleasedThisFrame && player.playerData.grenadeCanceled
                     || mouse.rightButton.isPressed && player.playerData.grenadeCanceled)
                {
                    Debug.LogWarning("canceled");
                    return;
                }
                    Debug.LogWarning("explosion");
                    SetCooldownOf(grenadeImage);
               }
                
            

            if (keybord.digit1Key.wasPressedThisFrame 
                && Inventory.instance.GetEquipmentByType(EquipmentType.Flask)!= null)
            {
                SetCooldownOf(flaskImage);
            }
        }
        CheckCoolDownOf(flaskImage,Inventory.instance.flaskCooldown);
        CheckCoolDownOf(dashImage, skills.dashSkill.cooldown);
        CheckCoolDownOf(parryImage, skills.parrySkill.cooldown);
        CheckCoolDownOf(blackholeImage,skills.blackholeSkill.cooldown);
        CheckCoolDownOf(grenadeImage, skills.grenadeSkill.cooldown);
    }

    private void UpdateSoulUI()
    {
        if(soulAmount < PlayerManager.instance.CurrentCurrencyAmount())
            soulAmount = Time.deltaTime * increaseRate;
        else 
            soulAmount = PlayerManager.instance.CurrentCurrencyAmount();

        currentSouls.text = ((int)soulAmount).ToString();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }

    private void CheckCoolDownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}
