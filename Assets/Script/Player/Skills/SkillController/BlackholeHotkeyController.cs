// using UnityEngine;
// using TMPro;
// using UnityEngine.InputSystem;
// public class BlackholeHotkeyController : MonoBehaviour
// {
//    private KeyCode myHotKey;
//    private TextMeshProUGUI myText;
//
//    public void SetHotKey(KeyCode _hotKey)
//    {
//       myText = GetComponentInChildren<TextMeshProUGUI>();
//       
//       myText.text = myHotKey.ToString();
//       myHotKey = _hotKey;
//    }
//
//    private void Update()
//    {
//       if (Input.GetKeyDown(myHotKey))
//       {
//          Debug.Log("hot key is"+ myHotKey);
//       }
//    }
// }


using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

using UnityEngine.InputSystem;


public class BlackholeHotkeyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Keyboard keyboard;
    private TextMeshProUGUI myText;
    private Key key = Key.Numpad0;
    private PlayerInput playerInput;
    private InputAction blackholeAction;

    private Transform myEnemies;
    
    private BlackholeSkillController blackhole;
    private void Awake()
    {
        keyboard = Keyboard.current;
        // 初始化 TextMeshProUGUI 组件
        
        blackhole = GameObject.FindGameObjectWithTag("Blackhole").GetComponent<BlackholeSkillController>();
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        playerInput = PlayerManager.instance?.player?.playerInput;
        if (playerInput != null)
        {
            blackholeAction = playerInput.actions.FindActionMap("PlayerSkills")?.FindAction("Blackhole");
            if (blackholeAction != null)
            {
                Debug.Log("blackholeAction is not null");
            }
            else
            {
                Debug.Log("blackholeAction is null");
                blackholeAction = playerInput.actions.FindActionMap("PlayerSkills")?.FindAction("Blackhole");
            }
        }
        else
        {
            Debug.Log("playerinput is null");
            playerInput = PlayerManager.instance?.player?.playerInput;
        }
        
       
    }

    private void Start()
    {
        StartCoroutine(InitializeAfterDelay());
    }
    public void SetHotKey(Key _key,Transform _myEnemies,BlackholeSkillController _myBlackhole)
    {
        Debug.Log("sethotkey"+_key);
        
        myEnemies = _myEnemies;
        blackhole = _myBlackhole;
        key = _key;
        // 确认hotkeyAction是否配置
        if (blackholeAction != null)
        {
            
            blackholeAction.ChangeBindingWithPath("<Keyboard>/" + key.ToString().ToLower());
            if (myText != null)
            {
                myText.text = _key.ToString();
            }
        }
    }
    private void EnsurePlayerInput()
    {
        if (playerInput == null && PlayerManager.instance != null)
        {
            playerInput = PlayerManager.instance.player?.playerInput;
        }
    }
    private IEnumerator InitializeAfterDelay()
    {
        yield return new WaitForEndOfFrame(); // 或更长的延迟
        EnsurePlayerInput();
        if (playerInput != null)
        {
            blackholeAction = playerInput.actions.FindActionMap("PlayerSkills")?.FindAction("Blackhole");
            if (blackholeAction != null)
            {
                blackholeAction.Enable();
            }
            else
            {
                Debug.LogWarning("Failed to find 'Blackhole' action.");
            }
        }
        else
        {
            Debug.LogWarning("Player Input is still not assigned after delay.");
        }
    }
    

    
    private void OnEnable()
    {
        EnsurePlayerInput();
        if (playerInput != null)
        {
            blackholeAction = playerInput.actions.FindActionMap("PlayerSkills")?.FindAction("Blackhole");
            if (blackholeAction != null)
            {
                blackholeAction.performed += GetKeyInput;
                blackholeAction.Enable();
            }
            else
            {
                Debug.LogWarning("Failedto find 'Blackhole' action.");
            }
        }
        else
        {
            Debug.LogWarning("Player Input is not assigned.");
        }
        
        
       
    }

    private void OnDisable()
    {
        EnsurePlayerInput();
        if (playerInput != null)
        {
            blackholeAction = playerInput.actions.FindActionMap("PlayerSkills")?.FindAction("Blackhole");
            if (blackholeAction != null)
            {
                blackholeAction.performed -= GetKeyInput;
                blackholeAction.Disable();
            }
            else
            {
                Debug.LogWarning("Failedto find 'Blackhole' action.");
            }
        }
        else
        {
            Debug.LogWarning("Player Input is not assigned.");
        }
        
    }

    
       
        private void GetKeyInput(InputAction.CallbackContext ctx)
        {
            if (Keyboard.current == null)
            {
                Debug.LogWarning("Keyboard or key is null.");
                return;
            }

            // 验证键是否在有效范围内
            if (!IsKeyValid(key)|| key == Key.None)
            {
                Debug.LogError("Key is out of range or not supported: " + key);
                return;
            }

            if (Keyboard.current[key].IsPressed())
            {
                Debug.Log("Get Key Input: " + key.ToString());
                blackhole.AddEnemyToList(myEnemies); // 确保 blackhole 和 myEnemies 定义和初始化
                myText.color = Color.clear; // 确保 myText 已定义
                sr.color = Color.clear; // 确保 sr 已定义
            }
        }

        private bool IsKeyValid(Key _key)
        {
            // 假设所有键在 Key 枚举的范围内是有效的
            var validKeys = new HashSet<Key>((Key[])Enum.GetValues(typeof(Key)));
            return validKeys.Contains(_key);
        }
    }
