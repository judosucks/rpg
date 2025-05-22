
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private float crystalExistTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;
    [SerializeField] private float growSpeed;
    private bool canGrow;
    private Player player;
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Player _player)
    {
        player = _player;
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
    }
   

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCryStal();
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,new Vector2(3,3),growSpeed * Time.deltaTime);
        }
        
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(transform.position, cd.radius);
        
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Debug.Log("enemy hit from crystal");
                player.stats.DoMagicDamage(hit.GetComponent<CharacterStats>());
            }
        }
    }
    public void FinishCryStal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
        
    }

    public void SelfDestroy() =>Destroy(gameObject);
}
