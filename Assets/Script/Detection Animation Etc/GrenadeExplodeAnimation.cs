using UnityEngine;
using UnityEngine.Tilemaps;
public class GrenadeExplodeAnimation : MonoBehaviour
{
    private Player player;
    private PlayerData playerData;
    [SerializeField] private LayerMask destructibleLayer;
    [SerializeField] private LayerMask enemyLayer;

    private void Start()
    {
        player = PlayerManager.instance.player;
        if (player != null)
        {
            Debug.Log("not null");
            playerData = player.playerData;
        }else if (playerData == null || player == null)
        {
            Debug.LogError("fuck null null");
        }
      
        Explode(); // Call Explode at the start of the animation
    }

    private void Explode()
    {
        // Damage Tilemap
        Collider2D[] hitTileColliders = Physics2D.OverlapCircleAll(transform.position, player.playerData.explosionRadius, destructibleLayer);
        foreach (Collider2D hit in hitTileColliders)
        {
            TilemapDamageTest tilemapDamageTest = hit.GetComponentInParent<TilemapDamageTest>();
            if (tilemapDamageTest != null)
            {
                Vector3 hitPoint = hit.ClosestPoint(transform.position);
                for (int i = 0; i < tilemapDamageTest.destructibleLayers.Length; i++)
                {
                    Vector3Int tilePos = tilemapDamageTest.destructibleLayers[i].WorldToCell(hitPoint);
                    tilemapDamageTest.DamageTile(tilePos);
                }
            }
        }
        // Damage Enemies
        Collider2D[] hitEnemyColliders = Physics2D.OverlapCircleAll(transform.position, player.playerData.explosionRadius, enemyLayer);
        foreach (Collider2D hit in hitEnemyColliders)
        {
            EnemyStats target = hit.GetComponent<EnemyStats>();
            if (target != null)
            {
                player.stats.DoDamage(target);
            }
        }
    }

    private void OnExplosionEffectComplete()
    {
        Debug.Log("destroying grenade");
        Destroy(gameObject);
    }
        
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, player.playerData.explosionRadius);
    }
}