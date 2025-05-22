using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UIHealthBar : MonoBehaviour
{
    private Entity entity;
    private RectTransform rectTransform;
    private Slider slider;
    private CharacterStats myStats;
    private void Start()
    {
        myStats = GetComponentInParent<CharacterStats>();
        rectTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        entity.onFlipped += FlipUi;
        myStats.OnHealthChanged += UpdateHealthUI;
    }

    

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }
    private void FlipUi()
    {
        rectTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUi;
        myStats.OnHealthChanged -= UpdateHealthUI;
    }
}
