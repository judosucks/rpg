using System;
using System.Collections;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainLevel1";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UIScreenFade screenFade;
    private void Start()
    {
 
        if(SaveManager.instance.HasSavedData() == false)
            continueButton.SetActive(false);
        
    }

    public void ContinueGame()
    {
        // 继续游戏的逻辑
        Debug.Log("Continue Game");
        // 这里可以添加继续游戏的代码，例如加载上次保存的场景
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }
    public void NewGame()
    {
        // 新游戏的逻辑
        Debug.Log("New Game");
        // 这里可以添加新游戏的代码，例如加载新场景
        SaveManager.instance.DeleteSavedGame();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }
    public void ExitGame()
    {
        // 退出游戏的逻辑
        Debug.Log("Exit Game");
        // 这里可以添加退出游戏的代码，例如保存数据等
        Application.Quit();
    }
    IEnumerator LoadSceneWithFadeEffect(float fadeDuration)
    {
        // Fade out
        screenFade.FadeOut();
        yield return new WaitForSeconds(fadeDuration);
        
        // Load the scene
        SceneManager.LoadScene(sceneName);
    }
}
