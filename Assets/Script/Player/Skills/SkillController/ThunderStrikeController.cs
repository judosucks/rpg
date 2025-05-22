using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;

    private Animator anim;
    private bool triggered;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();    
    }


    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    // Update is called once per frame
    void Update()
    {
        if(!targetStats)
            return;


        if (triggered)
            return;


        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            anim.transform.localPosition = new Vector3(0, .9f);
            anim.transform.localRotation = Quaternion.identity;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(1, 1);



            Invoke("DamageAndSelfDestroy", .2f);
            triggered = true;
            anim.SetTrigger("LightningHit");
        }
    }

    private void DamageAndSelfDestroy()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("thunder"))
        {
            float thunderDuration = stateInfo.length;
            targetStats.TakeDamage(damage);
            Destroy(gameObject, thunderDuration);
            
        }
    }
}
