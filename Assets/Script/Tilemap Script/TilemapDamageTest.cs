using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
public class TilemapDamageTest : MonoBehaviour
{    
    public Tilemap[] destructibleLayers; // 可破坏的Tilemap层
    [SerializeField] private GameObject debrisEffect;
    [SerializeField] private float destroyDelay = 1f; // 特效自动销毁时间

    public void DamageTile(Vector3Int position)
    {
        foreach (Tilemap layer in destructibleLayers)
        {
            if (layer.HasTile(position))
            {
                layer.SetTile(position, null);
                SpawnDebrisEffect(layer, position);
                break; // 只破坏第一个找到Tile的层级
            }
        }
    }

    private void SpawnDebrisEffect(Tilemap targetLayer, Vector3Int position)
    {
        if (debrisEffect == null) return;

        Vector3 worldPos = targetLayer.GetCellCenterWorld(position);
        // GameObject effect = Instantiate(debrisEffect, worldPos, Quaternion.identity);
        // Destroy(effect, destroyDelay); // 自动清理
        GameObject effect = DebrisPool.Instance.GetEffect();
        effect.transform.position = worldPos;
       

// 添加自动返还逻辑
        StartCoroutine(ReturnEffectAfterTime(effect, destroyDelay));
    }
    IEnumerator ReturnEffectAfterTime(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        DebrisPool.Instance.ReturnEffect(effect);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        foreach (Tilemap layer in destructibleLayers)
        {
            Gizmos.DrawWireCube(layer.localBounds.center, layer.localBounds.size);
        }
    }
}