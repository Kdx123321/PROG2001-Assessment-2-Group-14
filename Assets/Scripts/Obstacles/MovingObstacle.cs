using UnityEngine;

/// <summary>
/// 移动障碍物脚本 - 用于创建左右移动的障碍物
/// 可用于增加游戏难度
/// </summary>
public class MovingObstacle : MonoBehaviour
{
    [Header("移动设置")]
    public float moveDistance = 2f;      // 移动距离
    public float moveSpeed = 2f;         // 移动速度
    public bool moveHorizontal = true;   // true=水平移动, false=垂直移动

    private Vector3 startPos;
    private float direction = 1f;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // 计算新位置
        Vector3 newPos;
        if (moveHorizontal)
        {
            newPos = startPos + Vector3.right * Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        }
        else
        {
            newPos = startPos + Vector3.up * Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        }
        
        transform.position = newPos;
    }

    /// <summary>
    /// 碰撞检测
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 通知玩家游戏结束
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                // 通过碰撞器触发游戏结束
            }
        }
    }
}
