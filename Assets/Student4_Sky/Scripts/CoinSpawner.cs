using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("金币预制体")]
    public GameObject coinPrefab;

    [Header("金币数量")]
    public int coinCount = 10;

    [Header("生成范围")]
    public float spawnRange = 20f;

    [Header("最小间距")]
    public float minDistance = 3f;

    [Header("Y轴高度")]
    public float spawnHeight = 0.5f;

    void Start()
    {
        SpawnCoins();
    }

    void SpawnCoins()
    {
        int spawned = 0;
        int attempts = 0;
        int maxAttempts = coinCount * 10;

        while (spawned < coinCount && attempts < maxAttempts)
        {
            attempts++;
            Vector3 spawnPos = GetRandomPosition();
            
            // 检查位置是否有效
            if (IsPositionValid(spawnPos))
            {
                GameObject coin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);
                coin.name = "Coin_" + spawned;
                spawned++;
            }
        }

        Debug.Log("生成了 " + spawned + " 个金币");
        
        // 更新游戏管理器的金币总数
        if (SkyGameManager.Instance != null)
        {
            SkyGameManager.Instance.totalCoins = spawned;
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnRange, spawnRange);
        float z = Random.Range(-spawnRange, spawnRange);
        return new Vector3(x, spawnHeight, z);
    }

    bool IsPositionValid(Vector3 position)
    {
        // 检查是否太靠近原点（避免与玩家重叠）
        if (Vector3.Distance(position, Vector3.zero) < minDistance)
            return false;

        // 检查是否与其他金币太近
        Collider[] colliders = Physics.OverlapSphere(position, minDistance);
        foreach (Collider col in colliders)
        {
            if (col.name.Contains("Coin") || col.name.Contains("CloudObstacle"))
                return false;
        }

        return true;
    }
}
