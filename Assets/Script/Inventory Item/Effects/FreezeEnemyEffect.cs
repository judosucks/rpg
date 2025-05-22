using UnityEngine;
[CreateAssetMenu(fileName = "Freeze Enemy Effect", menuName = "Item/Item Effect/Freeze Enemy")]
public class FreezeEnemyEffect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExcutedEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
        {
            return;
        }
        if (!Inventory.instance.CanUseArmor())
        {
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
