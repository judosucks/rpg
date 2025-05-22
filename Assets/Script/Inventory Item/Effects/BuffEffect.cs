using UnityEngine;
using Yushan.Enums;
[CreateAssetMenu(fileName = "Buff Effect", menuName = "Item/Item Effect/Buff Effect")]
public class BuffEffect : ItemEffect
{
    private PlayerStats playerStats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExcutedEffect(Transform _enemyPosition)
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.IncreaseStatBy(buffAmount,buffDuration,playerStats.StatToModify(buffType));
    }

    
}
