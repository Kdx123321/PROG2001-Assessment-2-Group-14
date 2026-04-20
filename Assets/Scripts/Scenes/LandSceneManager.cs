using UnityEngine;

/// <summary>
/// 陆地场景管理器 - 学生3负责
/// 特点：有一整段障碍物，需要跳跃来躲避
/// </summary>
public class LandSceneManager : MonoBehaviour
{
    [Header("场景设置")]
    public GameObject highObstaclePrefab; // 高障碍物预制体（需要跳跃躲避）
    public GameObject rockPrefab;         // 岩石预制体
    public GameObject coinPrefab;         // 金币预制体
    public GameObject bigCoinPrefab;      // 大金币预制体

    [Header("生成设置")]
    public float spawnDistance = 100f;    // 场景长度
    public float sectionLength = 30f;     // 每个路段长度
    public float minSpawnZ = 20f;         // 开始生成的Z位置

    [Header("陆地环境")]
    public Material groundMaterial;       // 地面材质
    public Material rockMaterial;         // 岩石材质
    public GameObject[] landDecorationPrefabs; // 陆地装饰（仙人掌、石头等）

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
        
        while (currentZ < spawnDistance - 20)
        {
            // 随机选择生成模式
            float random = Random.value;
            
            if (random > 0.6f)
            {
                // 生成需要跳跃的障碍区域
                GenerateJumpSection(currentZ);
                currentZ += 15f;
            }
            else if (random > 0.3f)
            {
                // 生成普通金币
                GenerateCoinRow(currentZ);
                currentZ += 8f;
            }
            else
            {
                // 生成可收集的金币弧线（需要跳跃获取）
                GenerateCoinArc(currentZ);
                currentZ += 10f;
            }
        }
        
        // 在终点生成大金币
        SpawnBigCoin(spawnDistance);
    }

    /// <summary>
    /// 生成需要跳跃的障碍区域
    /// </summary>
    void GenerateJumpSection(float startZ)
    {
        // 生成高障碍物，玩家必须跳跃才能通过
        int obstacleLane = Random.Range(0, 3); // 随机选择一条跑道放置障碍
        float x = (obstacleLane - 1) * 3f;
        
        // 生成障碍物
        SpawnHighObstacle(x, startZ);
        
        // 在其他跑道生成金币
        for (int lane = 0; lane < 3; lane++)
        {
            if (lane != obstacleLane)
            {
                float coinX = (lane - 1) * 3f;
                SpawnCoins(coinX, startZ);
            }
        }
        
        // 在障碍上方放置金币（完美跳跃可以收集）
        SpawnHighCoin(x, startZ + 2f);
    }

    /// <summary>
    /// 生成高障碍物
    /// </summary>
    void SpawnHighObstacle(float x, float z)
    {
        if (highObstaclePrefab != null)
        {
            // 高障碍物高度2米，玩家可以通过跳跃越过
            Vector3 position = new Vector3(x, 1f, z);
            GameObject obstacle = Instantiate(highObstaclePrefab, position, Quaternion.identity);
            obstacle.tag = "Obstacle";
            
            // 设置障碍物高度
            obstacle.transform.localScale = new Vector3(1.5f, 2f, 0.5f);
            
            // 添加碰撞器
            if (obstacle.GetComponent<Collider>() == null)
            {
                BoxCollider bc = obstacle.AddComponent<BoxCollider>();
                bc.size = new Vector3(1.5f, 2f, 0.5f);
            }
        }
    }

    /// <summary>
    /// 生成高处金币
    /// </summary>
    void SpawnHighCoin(float x, float z)
    {
        if (coinPrefab != null)
        {
            // 在障碍物上方放置金币
            Vector3 position = new Vector3(x, 3f, z);
            GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);
            coin.tag = "Coin";
            
            if (coin.GetComponent<Collider>() == null)
            {
                SphereCollider sc = coin.AddComponent<SphereCollider>();
                sc.isTrigger = true;
            }
        }
    }

    /// <summary>
    /// 生成一排金币
    /// </summary>
    void GenerateCoinRow(float z)
    {
        int lane = Random.Range(0, 3);
        float x = (lane - 1) * 3f;
        
        for (int i = 0; i < 4; i++)
        {
            SpawnCoins(x, z + i * 2f);
        }
    }

    /// <summary>
    /// 生成金币弧线（需要跳跃获取）
    /// </summary>
    void GenerateCoinArc(float startZ)
    {
        int lane = Random.Range(0, 3);
        float x = (lane - 1) * 3f;
        
        // 创建弧线形状的金币
        for (int i = 0; i < 5; i++)
        {
            float z = startZ + i * 2f;
            float y = 1f + Mathf.Sin((float)i / 4 * Mathf.PI) * 2f; // 弧线高度
            
            if (coinPrefab != null)
            {
                Vector3 position = new Vector3(x, y, z);
                GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);
                coin.tag = "Coin";
                
                if (coin.GetComponent<Collider>() == null)
                {
                    SphereCollider sc = coin.AddComponent<SphereCollider>();
                    sc.isTrigger = true;
                }
            }
        }
    }

    /// <summary>
    /// 生成金币
    /// </summary>
    void SpawnCoins(float x, float z)
    {
        if (coinPrefab != null && Random.value > 0.2f)
        {
            Vector3 position = new Vector3(x, 1f, z);
            GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);
            coin.tag = "Coin";
            
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
            
            if (bigCoin.GetComponent<Collider>() == null)
            {
                SphereCollider sc = bigCoin.AddComponent<SphereCollider>();
                sc.isTrigger = true;
                sc.radius = 1.5f;
            }
        }
    }

    /// <summary>
    /// 设置环境
    /// </summary>
    void SetupEnvironment()
    {
        // 生成地面纹理变化
        for (float z = 0; z <= spawnDistance; z += 20)
        {
            // 在两侧生成装饰
            if (landDecorationPrefabs.Length > 0)
            {
                if (Random.value > 0.5f)
                {
                    int randomIndex = Random.Range(0, landDecorationPrefabs.Length);
                    Vector3 leftPos = new Vector3(-7f + Random.Range(-2f, 0f), 0, z + Random.Range(-5f, 5f));
                    Instantiate(landDecorationPrefabs[randomIndex], leftPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
                }
                
                if (Random.value > 0.5f)
                {
                    int randomIndex = Random.Range(0, landDecorationPrefabs.Length);
                    Vector3 rightPos = new Vector3(7f + Random.Range(0f, 2f), 0, z + Random.Range(-5f, 5f));
                    Instantiate(landDecorationPrefabs[randomIndex], rightPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
                }
            }
        }
    }
}
