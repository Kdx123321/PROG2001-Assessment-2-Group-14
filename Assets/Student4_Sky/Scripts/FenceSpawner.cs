using UnityEngine;

public class FenceSpawner : MonoBehaviour
{
    [Header("围墙预制体")]
    public GameObject fencePrefab;

    [Header("围墙范围")]
    public float fenceRange = 25f;

    [Header("围墙高度")]
    public float fenceHeight = 2f;

    [Header("每边围墙数量")]
    public int fencesPerSide = 20;

    void Start()
    {
        SpawnFences();
    }

    void SpawnFences()
    {
        float step = (fenceRange * 2) / fencesPerSide;

        // 生成四边围墙
        // 上边 (Z = fenceRange)
        for (int i = 0; i <= fencesPerSide; i++)
        {
            float x = -fenceRange + i * step;
            CreateFence(new Vector3(x, fenceHeight / 2, fenceRange), Vector3.zero, step);
        }

        // 下边 (Z = -fenceRange)
        for (int i = 0; i <= fencesPerSide; i++)
        {
            float x = -fenceRange + i * step;
            CreateFence(new Vector3(x, fenceHeight / 2, -fenceRange), Vector3.zero, step);
        }

        // 左边 (X = -fenceRange)
        for (int i = 1; i < fencesPerSide; i++)
        {
            float z = -fenceRange + i * step;
            CreateFence(new Vector3(-fenceRange, fenceHeight / 2, z), new Vector3(0, 90, 0), step);
        }

        // 右边 (X = fenceRange)
        for (int i = 1; i < fencesPerSide; i++)
        {
            float z = -fenceRange + i * step;
            CreateFence(new Vector3(fenceRange, fenceHeight / 2, z), new Vector3(0, 90, 0), step);
        }
    }

    void CreateFence(Vector3 position, Vector3 rotation, float width)
    {
        GameObject fence = Instantiate(fencePrefab, position, Quaternion.Euler(rotation));
        fence.name = "Fence";
        // 宽度填满间距，高度贴地，厚度1
        fence.transform.localScale = new Vector3(width, fenceHeight, 1);
    }
}
