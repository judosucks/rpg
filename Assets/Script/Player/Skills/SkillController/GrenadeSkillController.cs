using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

public class GrenadeSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D _circleCollider2D;
    private Player player;
    private GrenadeSkill grenadeSkill;
    

    [Header("grenade info")] 
    [SerializeField]private GameObject grenadeExplodeFxPrefab;

    [SerializeField] private float grenadeUseDuration = 3f;
    [SerializeField]private int grenadeAmount;
    private float defaultGravityScale;
    private bool canRotate = false;
    public bool explosion { get; private set; }
    
    
    private SpriteRenderer sr;
    
    
    public bool isFlashing { get; private set; }
    
    [Header("frag grenade")] 
    private List<Transform> enemyTarget;
    private bool isFragGrenade;
    [Header("flash grenade")] 
    private bool isFlashGrenade;
    private int targetIndex;
    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        grenadeSkill = SkillManager.instance.grenadeSkill;
        
    }

    private void Start()
    {
        defaultGravityScale = rb.gravityScale;
     
    }

    private void ExplodeGrenade()
    {
        grenadeUseDuration -= Time.deltaTime;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Debug.Log("explode grenade from grenadeskillcontroller");
        GameObject explodeFx = Instantiate(grenadeExplodeFxPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        
        Animator anim = explodeFx.GetComponent<Animator>();
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        anim.SetBool("GrenadeExplodeFx",true);
        if (stateInfo.IsName("GrenadeExplodeFx") && stateInfo.normalizedTime >= 1f)
        {
            Debug.Log("destory grenade explode fx from grenadeskillcontroller");
            Destroy(explodeFx);
            explosion = false;
        }
        else
        {
            Debug.Log("grenade explode fx is not exploding");
        }
        
    }
    private IEnumerator CoundownToExplode()
    {
        Debug.Log("[CoundownToExplode] Countdown started...");
        
        anim.SetBool("Rotation", true); 
        
        float elapsedTime = 0f;

        // Maximum and minimum flash rates
        float maxFlashRate = 5f; // Flash 5 times per second when close to exploding
        float minFlashRate = 0.5f; // Flash every 2 seconds at the beginning

        while (elapsedTime < grenadeSkill.explosionTimer)
        {
            float remainingTime = grenadeSkill.explosionTimer - elapsedTime;

            // Dynamically calculate flash rate based on remaining time
            float flashRate = Mathf.Lerp(maxFlashRate, minFlashRate, remainingTime / grenadeSkill.explosionTimer); // Linearly interpolate
            
           // Flash effect starts here
            if (isFlashing)
            {
                StartCoroutine(GrenadeFlashFx(0.5f / flashRate));
            }

            // Wait based on the calculated flash rate
            yield return new WaitForSeconds(0.5f / flashRate);

            elapsedTime += 1 / flashRate; // Increment elapsed time based on flash
        }
        isFlashing = false;
        explosion = true;
    }
    
    public IEnumerator GrenadeFlashFx(float flashInterval)
    {
        // Set grenade to flash on and off
        Renderer rend = GetComponentInChildren<Renderer>();
        // Flash on
        rend.material.color = Color.red; // Example color when flashing
        yield return new WaitForSeconds(flashInterval / 2);
        
        // Flash off
        rend.material.color = Color.white; // Default color
        yield return new WaitForSeconds(flashInterval / 2);
    }
    private void Update()
    {
        
        if (explosion)
        {
            ExplodeGrenade();
        }
       
    }

    public void SetupGrenade(Vector2 _direction, float _gravity, Player _player)
    {
        Debug.Log("设置手榴弹方向和引力");
        // Existing initialization logic...
        player = _player ?? PlayerManager.instance.player; // 确保 player 不为空
        rb = rb ?? GetComponent<Rigidbody2D>();
        anim = anim ?? GetComponentInChildren<Animator>();
        _circleCollider2D = _circleCollider2D ?? GetComponent<CircleCollider2D>();
        rb.linearVelocity = _direction;
        rb.gravityScale = _gravity;
        
        Debug.Log("next up ");
            
        
        isFlashing = true; 
        StartCoroutine(CoundownToExplode()); // 启动爆炸协程
    }
    
    public void SetupFragGrenade(bool _isFragGrenade)
    {
        Debug.Log("set up frag grenade");
        isFragGrenade = _isFragGrenade;
        enemyTarget = new List<Transform>();
    }

    public void SetupFlashGrenade(bool _isFlashGrenade)
    {
        Debug.Log("set up flash grenade");
        isFlashGrenade = _isFlashGrenade;
        enemyTarget = new List<Transform>();
    }
    public void ReadyToUseGrenade()
    {
        if (grenadeUseDuration < 0)
        {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Enemy"))
        {
            // 减少弹跳力
            // 调整因子以获得所需的反弹效果
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,Mathf.Abs(rb.linearVelocity.y) * 0.7f); // 例如，把垂直速度减少到原来的70%
            // 其他与反弹相关的逻辑
        }
    }
}
