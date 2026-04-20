using UnityEngine;

/// <summary>
/// 河流场景管理器 - 学生2负责
/// 特点：有一整段障碍物，需要下滑来躲避
/// </summary>
public class RiverSceneManager : MonoBehaviour
{
    [Header("场景设置")]
    public GameObject lowObstaclePrefab; // 低矮障碍物预制体（需要下滑躲避）
    public GameObject bridgePrefab;      // 桥梁预制体
    public GameObject coinPrefab;        // 金币预制体
    public GameObject bigCoinPrefab;     // 大金币预制体

    [Header("生成设置")]
    public float spawnDistance = 100f;   // 场景长度
    public float sectionLength = 30f;    // 每个河段长度
    public float minSpawnZ = 20f;        // 开始生成的Z位置

    [Header("河流环境")]
    public Material waterMaterial;       // 水面材质
    public Material groundMaterial;      // 地面材质
    public GameObject[] waterDecorationPrefabs; // 水面装饰（荷叶、石头等）

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
        
        // 生成正常路段
        while (currentZ < spawnDistance - 20)
        {
            // 每隔一段距离生成一个需要下滑的障碍区域
            if (Random.value > 0.6f)
            {
                // 生成低矮障碍区域（需要下滑）
                GenerateLowObstacleSection(currentZ);
                currentZ += 20f; // 障碍区域长度
            }
            else
            {
                // 生成正常金币
                GenerateNormalCoins(currentZ);
                currentZ += 10f;
            }
        }
        
        // 在终点生成大金币
        SpawnBigCoin(spawnDistance);
    }

    /// <summary>
    /// 生成低矮障碍物区域 - 玩家必须下滑才能通过
    /// </summary>
    void GenerateLowObstacleSection(float startZ)
    {
        // 生成横跨三条跑道的低矮障碍
        for (float z = startZ; z < startZ + 15f; z += 3f)
        {
            // 左道障碍
            SpawnLowObstacle(-3f, z);
            // 中道障碍
            SpawnLowObstacle(0f, z);
            // 右道障碍
            SpawnLowObstacle(3f, z);
        }
        
        // 在障碍区域前提示玩家
        SpawnWarningSign(startZ - 10f);
    }

    /// <summary>
    /// 生成低矮障碍物
    /// </summary>
    void SpawnLowObstacle(float x, float z)
    {
        if (lowObstaclePrefab != null)
        {
            // 障碍物高度较低（0.8米），玩家站立时会撞到，需要下滑
            Vector3 position = new Vector3(x, 0.4f, z);
            GameObject obstacle = Instantiate(lowObstaclePrefab, position, Quaternion.identity);
            obstacle.tag = "Obstacle";
            
            // 缩放障碍物使其变低
            obstacle.transform.localScale = new Vector3(1f, 0.8f, 1f);
            
            // 添加碰撞器
            if (obstacle.GetComponent<Collider>() == null)
            {
                BoxCollider bc = obstacle.AddComponent<BoxCollider>();
                bc.size = new Vector3(1f, 0.8f, 1f);
            }
        }
    }

    /// <summary>
    /// 生成警告标志
    /// </summary>
    void SpawnWarningSign(float z)
    {
        // 可以在这里生成提示标志，告诉玩家需要下滑
        // 比如一个向下的箭头标志
    }

    /// <summary>
    /// 生成普通金币
    /// </summary>
    void GenerateNormalCoins(float z)
    {
        // 随机选择一条跑道生成金币
        int lane = Random.Range(0, 3);
        float x = (lane - 1) * 3f;
        
        for (int i = 0; i < 3; i++)
        {
            if (coinPrefab != null)
            {
                Vector3 position = new Vector3(x, 1f, z + i * 2f);
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
        // 生成两侧的水面
        GameObject leftWater = GameObject.CreatePrimitive(PrimitiveType.Plane);
        leftWater.transform.position = new Vector3(-12f, -0.5f, spawnDistance / 2);
        leftWater.transform.localScale = new Vector3(3f, 1f, spawnDistance / 10);
        if (waterMaterial != null)
            leftWater.GetComponent<Renderer>().material = waterMaterial;
        Destroy(leftWater.GetComponent<Collider>()); // 移除碰撞器

        GameObject rightWater = GameObject.CreatePrimitive(PrimitiveType.Plane);
        rightWater.transform.position = new Vector3(12f, -0.5f, spawnDistance / 2);
        rightWater.transform.localScale = new Vector3(3f, 1f, spawnDistance / 10);
        if (waterMaterial != null)
            rightWater.GetComponent<Renderer>().material = waterMaterial;
        Destroy(rightWater.GetComponent<Collider>());
        
        // 生成水面装饰
        for (float z = 0; z <= spawnDistance; z += 15)
        {
            if (waterDecorationPrefabs.Length > 0 && Random.value > 0.6f)
            {
                int randomIndex = Random.Range(0, waterDecorationPrefabs.Length);
                Vector3 leftPos = new Vector3(-10f, 0, z);
                Instantiate(waterDecorationPrefabs[randomIndex], leftPos, Quaternion.identity);
            }
            
            if (waterDecorationPrefabs.Length > 0 && Random.value > 0.6f)
            {
                int randomIndex = Random.Range(0, waterDecorationPrefabs.Length);
                Vector3 rightPos = new Vector3(10f, 0, z);
                Instantiate(waterDecorationPrefabs[randomIndex], rightPos, Quaternion.identity);
            }
        }
    }
}
