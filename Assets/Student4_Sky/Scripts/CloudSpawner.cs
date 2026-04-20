using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("路障预制体")]
    public GameObject cloudPrefab;

    [Header("生成数量")]
    public int cloudCount = 10;

    [Header("生成范围")]
    public float spawnRange = 20f;

    [Header("最小间距")]
    public float minDistance = 3f;

    [Header("Y轴高度")]
    public float spawnHeight = 0.5f;

    void Start()
    {
        SpawnClouds();
    }

    void SpawnClouds()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            Vector3 spawnPos = GetRandomPosition();
            
            // 检查与其他物体的距离
            if (IsPositionValid(spawnPos))
            {
                GameObject cloud = Instantiate(cloudPrefab, spawnPos, Quaternion.identity);
                cloud.name = "CloudObstacle_" + i;
                
                // 随机缩放，让云朵看起来不同
                float scale = Random.Range(0.8f, 1.5f);
                cloud.transform.localScale = new Vector3(scale * 1.5f, scale, scale);
            }
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

        // 检查是否与其他路障太近
        Collider[] colliders = Physics.OverlapSphere(position, minDistance);
        foreach (Collider col in colliders)
        {
            if (col.name.Contains("CloudObstacle"))
                return false;
        }

        return true;
    }
}
