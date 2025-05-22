using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    private Keyboard keyboard;
    
    public UIScreenFade screenFade;
    [SerializeField]private GameObject endText;
    [SerializeField]private GameObject restartButton;
    [Space]
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    public UISkillTreeTooltip skillTreeTooltip;
    public UIItemTooltip itemTooltip;
    public UIStatTooltip statTooltip;
    public UICraftWindow craftWindow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        SwitchTo(skillTreeUI);
        screenFade.gameObject.SetActive(true);
    }

    void Start()
    {
        SwitchTo(inGameUI);
        keyboard = Keyboard.current;
        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
        
    }

    public void SwitchOnEndScreen()
    {
        screenFade.FadeOut();
        StartCoroutine(EndScreenCorutine());
    }

    IEnumerator EndScreenCorutine()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if(keyboard.cKey.wasPressedThisFrame)
            SwitchWithKeyTo(characterUI);
        if(keyboard.bKey.wasPressedThisFrame)
            SwitchWithKeyTo(craftingUI);
        if(keyboard.kKey.wasPressedThisFrame)
            SwitchWithKeyTo(skillTreeUI);
        if(keyboard.oKey.wasPressedThisFrame)
            SwitchWithKeyTo(optionsUI);
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
        bool fadeScreen = transform.GetChild(i).GetComponent<UIScreenFade>() != null;
            if(fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
            
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UIScreenFade>() == null)
            {
                return;
            }
        
            SwitchTo(inGameUI);
        }
    }

    public void RestartGameButton()
    {
        GameManager.instance.RestartScene();
    }
    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     UIInGame uiInGame = inGameUI.GetComponent<UIInGame>();
    //     if (uiInGame != null && uiInGame.optionMenuToggle != null)
    //     {
    //         if (uiInGame.optionMenuToggle.isOn)
    //         {
    //             SwitchWithKeyTo(optionsUI);
    //         }
    //         else if(!uiInGame.optionMenuToggle.isOn)
    //         {
    //             optionsUI.SetActive(false);
    //             inGameUI.SetActive(true);
    //         }
    //         
    //     }
    // }
}
