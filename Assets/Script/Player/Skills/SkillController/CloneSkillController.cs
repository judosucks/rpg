using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Yushan.Enums;
using Random = UnityEngine.Random;


public class CloneSkillController : MonoBehaviour
{
    private Player player;
    
    private SpriteRenderer sr;
    private Animator anim;
    private float attackMultiplier;
    [SerializeField] private float colorLoosingSpeed;
   
    [SerializeField]private Transform closestEnemy;
   
    [SerializeField]private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    int ATTACKNUMBER = Animator.StringToHash("AttackNumber");
    [SerializeField] private float Radius = 10f;
    private bool canDuplicateClone;
    private float chanceToDuplicate;
    private int facingDir = 1;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        

    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        
        if (cloneTimer < 0)
        {
            
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset, Transform _closestEnemy,bool _canDuplicate,float _chanceToDuplicate, Player _player, float _attackMultiplier)
    {
       
       
            if (_canAttack)
            {
                int Number_Attack = Random.Range(1, 5);
                anim.SetInteger(ATTACKNUMBER,Number_Attack);
                
            }

            attackMultiplier = _attackMultiplier;
            player = _player;
            
            transform.position = _newTransform.position + _offset;
            cloneTimer = _cloneDuration;
            canDuplicateClone = _canDuplicate;
            chanceToDuplicate = _chanceToDuplicate;
            closestEnemy = _closestEnemy;

            FaceClosestTarget();



    }
    private void AnimationTrigger()
    {
        Debug.Log("animation trigger from cloneskillcontroller");
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                // player.stats.DoDamage(hit.GetComponent<CharacterStats>());
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);
                if (player.skill.cloneSkill.canApplyOnHitEffect)
                {
                    ItemDataEquipment weaponData = Inventory.instance.GetEquipmentByType(EquipmentType.Weapon);
                    if (weaponData != null)
                    {
                        weaponData.ItemEffect(hit.transform);
                    }

                    if (canDuplicateClone)
                    {
                        if (Random.Range(0, 100) < chanceToDuplicate)
                        {
                            Debug.Log("duplicate clone");
                            SkillManager.instance.cloneSkill.CreateClone(hit.transform,
                                new Vector3(0.1f * facingDir, 0));
                        }
                    }
                }
            }
        }
    }


    private void FaceClosestTarget()
            {
                if (closestEnemy != null)
                {

                    if (transform.position.x > closestEnemy.position.x)
                    {
                        facingDir = -1;
                        Debug.Log("turn right ");
                        transform.Rotate(0, 180, 0);
                    }
                }

            }


        }
// private void OnDrawGizmos()
    // { 
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position,closestEnemyCheckRadius);
    //     Gizmos.DrawWireSphere(attackCheck.transform.position,attackCheckRadius);
    // }

