using UnityEngine;
using Yushan.Enums;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();
        player = PlayerManager.instance.player;
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        
    }

    protected override void Die()
    {
        Debug.Log("die from playerstats");
        base.Die();
        if (player != null)
        {
           player.Die();
           GetComponent<PlayerDrop>().GenerateDrop();
        }
        
        
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
        player.stateMachine.ChangeState(player.hurtState);
        ItemDataEquipment currentArmor = Inventory.instance.GetEquipmentByType(EquipmentType.Armor);
        if (currentArmor != null)
        {
            currentArmor.ItemEffect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        player.skill.dodgeSkill.DodgeWithMirage();
    }

    public void CloneDoDamage(CharacterStats _targetStats,float _multiplier)
    {
        if (CanAviodAttack(_targetStats))
        {
            return;
        }
        int totalDamage = damage.GetValue() + strength.GetValue();

        if (_multiplier > 0)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);
        }
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
           
        }
        
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }
}
