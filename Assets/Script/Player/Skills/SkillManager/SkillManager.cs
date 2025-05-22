
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public DashSkill dashSkill { get; private set;}
    public CloneSkill cloneSkill { get; private set;}
    
    public GrenadeSkill grenadeSkill { get; private set;}
    
    public BlackholeSkill blackholeSkill { get; private set;}
    
    public CrystalSkill crystalSkill { get; private set;}
    public ParrySkill parrySkill { get; private set;}
    public DodgeSkill dodgeSkill { get; private set;}
    private void Awake()
    {
        Debug.Log("SkillManager Awake");
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
            return;
        }
        instance = this;
       
    }

    private void Start()
    {
        dashSkill = GetComponent<DashSkill>();
        cloneSkill = GetComponent<CloneSkill>();
        grenadeSkill = GetComponent<GrenadeSkill>();
        blackholeSkill = GetComponent<BlackholeSkill>();
        crystalSkill = GetComponent<CrystalSkill>();
        parrySkill = GetComponent<ParrySkill>();
        dodgeSkill = GetComponent<DodgeSkill>();
    }
}
