using UnityEngine;

/// <summary>
/// 天空场景管理器 - 学生4负责
/// 特点：左右两侧有云朵障碍物，需要向另一侧躲避
/// </summary>
public class SkySceneManager : MonoBehaviour
{
    [Header("场景设置")]
    public GameObject cloudObstaclePrefab; // 云朵障碍物预制体
    public GameObject floatingPlatformPrefab; // 浮空平台预制体
    public GameObject coinPrefab;          // 金币预制体
    public GameObject bigCoinPrefab;       // 大金币预制体

    [Header("生成设置")]
    public float spawnDistance = 100f;     // 场景长度
    public float sectionLength = 20f;      // 每个路段长度
    public float minSpawnZ = 20f;          // 开始生成的Z位置

    [Header("天空环境")]
    public Material skyMaterial;           // 天空材质
    public Material cloudMaterial;         // 云朵材质
    public GameObject[] skyDecorationPrefabs; // 天空装饰（小鸟、气球等）

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
            int pattern = Random.Range(0, 3);
            
            switch (pattern)
            {
                case 0: // 左侧云朵墙
                    GenerateCloudWall(-1, currentZ); // -1表示左侧
                    currentZ += 15f;
                    break;
                    
                case 1: // 右侧云朵墙
                    GenerateCloudWall(1, currentZ); // 1表示右侧
                    currentZ += 15f;
                    break;
                    
                case 2: // 两侧都有云朵，中间通道
                    GenerateCloudPassage(currentZ);
                    currentZ += 12f;
                    break;
            }
        }
        
        // 在终点生成大金币
        SpawnBigCoin(spawnDistance);
    }

    /// <summary>
    /// 生成云朵墙 - 阻挡一侧跑道，玩家需要切换到另一侧
    /// </summary>
    /// <param name="side">-1=左侧, 1=右侧</param>
    void GenerateCloudWall(int side, float startZ)
    {
        // 确定要阻挡的跑道
        int blockedLane = (side == -1) ? 0 : 2; // 0=左, 2=右
        float blockedX = (blockedLane - 1) * 3f;
        
        // 生成云朵墙（阻挡特定跑道）
        for (float z = startZ; z < startZ + 15f; z += 3f)
        {
            SpawnCloudObstacle(blockedX, z);
            
            // 在旁边也生成云朵形成墙
            float adjacentX = blockedX + (side * 2f);
            SpawnCloudObstacle(adjacentX, z);
        }
        
        // 在畅通的跑道生成金币
        int openLane = (side == -1) ? 2 : 0; // 相反的跑道
        float openX = (openLane - 1) * 3f;
        for (float z = startZ; z < startZ + 15f; z += 3f)
        {
            SpawnCoins(openX, z);
        }
        
        // 在中间跑道也生成一些金币
        for (float z = startZ + 5f; z < startZ + 10f; z += 2f)
        {
            SpawnCoins(0f, z);
        }
    }

    /// <summary>
    /// 生成云朵通道 - 两侧有云朵，中间可以通行
    /// </summary>
    void GenerateCloudPassage(float startZ)
    {
        // 生成两侧云朵
        for (float z = startZ; z < startZ + 12f; z += 3f)
        {
            // 左侧云朵群
            SpawnCloudObstacle(-4f, z);
            SpawnCloudObstacle(-5f, z + 1.5f);
            
            // 右侧云朵群
            SpawnCloudObstacle(4f, z);
            SpawnCloudObstacle(5f, z + 1.5f);
        }
        
        // 在中间通道生成金币
        for (float z = startZ; z < startZ + 12f; z += 2f)
        {
            SpawnCoins(0f, z);
        }
    }

    /// <summary>
    /// 生成云朵障碍物
    /// </summary>
    void SpawnCloudObstacle(float x, float z)
    {
        if (cloudObstaclePrefab != null)
        {
            Vector3 position = new Vector3(x, Random.Range(0.5f, 2f), z);
            GameObject cloud = Instantiate(cloudObstaclePrefab, position, Quaternion.identity);
            cloud.tag = "Obstacle";
            
            // 随机缩放
            float scale = Random.Range(0.8f, 1.5f);
            cloud.transform.localScale = Vector3.one * scale;
            
            // 添加云朵动画组件
            CloudFloat cloudFloat = cloud.AddComponent<CloudFloat>();
            cloudFloat.floatSpeed = Random.Range(0.5f, 1.5f);
            cloudFloat.floatAmplitude = Random.Range(0.1f, 0.3f);
            
            // 添加碰撞器
            if (cloud.GetComponent<Collider>() == null)
            {
                SphereCollider sc = cloud.AddComponent<SphereCollider>();
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
        // 在背景生成装饰性云朵
        for (float z = 0; z <= spawnDistance + 50; z += 25)
        {
            // 远处云朵
            for (int i = 0; i < 3; i++)
            {
                float x = Random.Range(-20f, 20f);
                float y = Random.Range(8f, 15f);
                Vector3 pos = new Vector3(x, y, z);
                
                if (cloudObstaclePrefab != null)
                {
                    GameObject bgCloud = Instantiate(cloudObstaclePrefab, pos, Quaternion.identity);
                    bgCloud.transform.localScale = Vector3.one * Random.Range(2f, 4f);
                    
                    // 背景云朵不需要碰撞器
                    Collider col = bgCloud.GetComponent<Collider>();
                    if (col != null) Destroy(col);
                    
                    // 添加浮动动画
                    CloudFloat cf = bgCloud.GetComponent<CloudFloat>();
                    if (cf == null) cf = bgCloud.AddComponent<CloudFloat>();
                    cf.floatSpeed = Random.Range(0.2f, 0.5f);
                }
            }
        }
        
        // 生成天空装饰
        for (float z = 0; z <= spawnDistance; z += 30)
        {
            if (skyDecorationPrefabs.Length > 0 && Random.value > 0.6f)
            {
                int randomIndex = Random.Range(0, skyDecorationPrefabs.Length);
                float x = Random.Range(-8f, 8f);
                Vector3 pos = new Vector3(x, Random.Range(5f, 10f), z);
                Instantiate(skyDecorationPrefabs[randomIndex], pos, Quaternion.identity);
            }
        }
    }
}

/// <summary>
/// 云朵浮动动画组件
/// </summary>
public class CloudFloat : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatAmplitude = 0.2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
