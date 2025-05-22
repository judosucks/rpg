using System.Collections;
using UnityEngine;

public class DashShadowEffect : MonoBehaviour
{
    public GameObject shadowPrefab;//影子prefab
    public float shadowSpawnInterval = 0.1f;//生成影子的時間間隔
    public float shadowLifeTime = 0.5f;//影子持續時間
    private PlayerDashState dashState;// 引用玩家的Dash状态
    private void Start()
    {
        dashState = GetComponent<Player>().dashState; // 获取PlayerDashState
    }
    private void Update()
    {
        if (dashState != null && dashState.IsDashing)
        {
            Debug.Log("dashing effect");
            StartCoroutine(SpawnShadow());
        }
    }
    private IEnumerator SpawnShadow()
    {
        while (dashState.IsDashing)
        {
            GameObject shadow = Instantiate(shadowPrefab, transform.position, transform.rotation);
            Destroy(shadow, shadowLifeTime);
            yield return new WaitForSeconds(shadowSpawnInterval);
        }
    }
    
    
    
    
}
