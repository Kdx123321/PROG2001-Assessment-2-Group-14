using UnityEngine;

/// <summary>
/// 丛林场景管理器 - 学生1负责
/// 特点：左右两侧有树桩障碍物，玩家需要切换轨道躲避
/// </summary>
public class JungleSceneManager : MonoBehaviour
{
    [Header("场景设置")]
    public GameObject treeStumpPrefab;  // 树桩预制体
    public GameObject coinPrefab;       // 金币预制体
    public GameObject bigCoinPrefab;    // 大金币预制体
    public Transform playerStartPoint;  // 玩家起始位置

    [Header("生成设置")]
    public float spawnDistance = 100f;  // 场景长度
    public float obstacleSpacing = 15f; // 障碍物间距
    public float coinSpacing = 5f;      // 金币间距
    public float minSpawnZ = 20f;       // 开始生成的Z位置

    [Header("丛林环境")]
    public Material groundMaterial;     // 地面材质
    public Material sideTreeMaterial;   // 树材质
    public GameObject[] decorationPrefabs; // 装饰物（石头、草丛等）

    void Start()
    {
        GenerateScene();
        SetupEnvironment();
    }

    /// <summary>
    /// 生成场景内容
    /// </summary>
    void GenerateScene()
    {
        float currentZ = minSpawnZ;
        
        while (currentZ < spawnDistance)
        {
            // 随机选择生成模式
            int pattern = Random.Range(0, 3);
            
            switch (pattern)
            {
                case 0: // 左侧树桩
                    SpawnTreeStump(-3f, currentZ);
                    SpawnCoins(0f, currentZ);
                    SpawnCoins(3f, currentZ);
                    break;
                    
                case 1: // 右侧树桩
                    SpawnTreeStump(3f, currentZ);
                    SpawnCoins(-3f, currentZ);
                    SpawnCoins(0f, currentZ);
                    break;
                    
                case 2: // 中间金币，两侧树桩
                    SpawnTreeStump(-3f, currentZ);
                    SpawnTreeStump(3f, currentZ);
                    SpawnCoins(0f, currentZ);
                    break;
            }
            
            currentZ += obstacleSpacing;
        }
        
        // 在终点生成大金币
        SpawnBigCoin(spawnDistance);
    }

    /// <summary>
    /// 生成树桩障碍物
    /// </summary>
    void SpawnTreeStump(float x, float z)
    {
        if (treeStumpPrefab != null)
        {
            Vector3 position = new Vector3(x, 0.5f, z);
            GameObject stump = Instantiate(treeStumpPrefab, position, Quaternion.identity);
            stump.tag = "Obstacle";
            
            // 添加碰撞器
            if (stump.GetComponent<Collider>() == null)
            {
                stump.AddComponent<BoxCollider>();
            }
        }
    }

    /// <summary>
    /// 生成金币
    /// </summary>
    void SpawnCoins(float x, float z)
    {
        if (coinPrefab != null && Random.value > 0.3f)
        {
            Vector3 position = new Vector3(x, 1f, z);
            GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);
            coin.tag = "Coin";
            
            // 添加触发器
            if (coin.GetComponent<Collider>() == null)
            {
                SphereCollider sc = coin.AddComponent<SphereCollider>();
                sc.isTrigger = true;
            }
        }
    }

    /// <summary>
    /// 生成大金币
    /// </summary>
    void SpawnBigCoin(float z)
    {
        if (bigCoinPrefab != null)
        {
            Vector3 position = new Vector3(0f, 2f, z);
            GameObject bigCoin = Instantiate(bigCoinPrefab, position, Quaternion.identity);
            bigCoin.tag = "BigCoin";
            
            // 添加触发器
            if (bigCoin.GetComponent<Collider>() == null)
            {
                SphereCollider sc = bigCoin.AddComponent<SphereCollider>();
                sc.isTrigger = true;
                sc.radius = 1.5f;
            }
        }
    }

    /// <summary>
    /// 设置环境装饰
    /// </summary>
    void SetupEnvironment()
    {
        // 在场景两侧生成树木和装饰
        for (float z = 0; z <= spawnDistance + 20; z += 10)
        {
            // 左侧装饰
            if (Random.value > 0.5f && decorationPrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, decorationPrefabs.Length);
                Vector3 leftPos = new Vector3(-8f + Random.Range(-2f, 2f), 0, z);
                Instantiate(decorationPrefabs[randomIndex], leftPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
            }
            
            // 右侧装饰
            if (Random.value > 0.5f && decorationPrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, decorationPrefabs.Length);
                Vector3 rightPos = new Vector3(8f + Random.Range(-2f, 2f), 0, z);
                Instantiate(decorationPrefabs[randomIndex], rightPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
            }
        }
    }
}
