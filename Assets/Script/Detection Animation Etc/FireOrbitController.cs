using UnityEngine;

public class FireOrbitController : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab; // Fire prefab
    [SerializeField] private int fireCount = 10; // Number of fire animations
    [SerializeField] private float orbitRadius = 2f; // 公轉半徑
    [SerializeField] private float orbitSpeed = 30f; // 公轉速度（度每秒）

    private GameObject[] fireObjects; // Array to store references to the fire objects
    private float[] angles; // 每個火焰的初始角度

    private Transform blackholeTransform; // 黑洞的參考

    public void Initialize(Transform blackhole)
    {
        blackholeTransform = blackhole;

        // Create fire objects around the blackhole
        fireObjects = new GameObject[fireCount];
        angles = new float[fireCount];

        float angleStep = 360f / fireCount; // Divide 360 degrees by fire count

        for (int i = 0; i < fireCount; i++)
        {
            // 計算初始位置
            Vector3 spawnPosition = CalculateFirePosition(i * angleStep);

            // 實例化火焰
            GameObject fireInstance = Instantiate(firePrefab, spawnPosition, Quaternion.identity);

            // 確保火焰的縮放保持獨立，不受黑洞父物體的縮放影響
            fireInstance.transform.localScale = Vector3.one;
            fireInstance.SetActive(false);// 確保火焰初始不可見
            // 設置火焰為黑洞的子對象
            fireInstance.transform.SetParent(blackholeTransform, true);

            fireObjects[i] = fireInstance;
            angles[i] = i * angleStep;
            
        }
        SetFireObjectsActive(true);// 啟用火焰顯示
    }
    public void SetFireObjectsActive(bool active)
    {
        if (fireObjects == null) return;

        foreach (GameObject fireObject in fireObjects)
        {
            if (fireObject != null)
            {
                fireObject.SetActive(active); // 將火焰設置為可見（true）或不可見（false）
            }
        }
    }

    private void Update()
    {
        // 檢查是否已初始化
        if (fireObjects == null || blackholeTransform == null) return;

        for (int i = 0; i < fireObjects.Length; i++)
        {
            if (fireObjects[i] == null) continue;

            // 更新角度
            angles[i] += orbitSpeed * Time.deltaTime;
            angles[i] %= 360f;

            // 公轉位置計算
            Vector3 newPosition = CalculateFirePosition(angles[i]);
            fireObjects[i].transform.position = newPosition;

            // 確保火焰的縮放和旋轉保持原狀
            fireObjects[i].transform.localScale = Vector3.one;  // 防止縮放受父物體影響
            fireObjects[i].transform.rotation = Quaternion.identity; // 防止火焰旋轉
        }
    }

    private Vector3 CalculateFirePosition(float angle)
    {
        // 將角度轉換為弧度
        float radians = angle * Mathf.Deg2Rad;

        // 依據黑洞的位置，計算火焰的位置
        float x = blackholeTransform.position.x + Mathf.Cos(radians) * orbitRadius;
        float y = blackholeTransform.position.y + Mathf.Sin(radians) * orbitRadius;

        return new Vector3(x, y, blackholeTransform.position.z);
    }
}

