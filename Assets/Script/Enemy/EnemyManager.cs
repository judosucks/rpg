using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public Enemy enemy;

    private void Awake()
    {
        Debug.Log("who calls first awake from enemymanager");
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            
        }

        // 确保 player 已经赋值
        if (enemy == null)
        {
            enemy = FindFirstObjectByType<Enemy>(); // 或者使用您特定的init逻辑
        }
    }
}