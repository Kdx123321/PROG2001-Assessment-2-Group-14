using UnityEngine;

public class SkyCoin : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        // 金币旋转动画
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        // 检测是否被玩家碰到
        if (other.name == "Cube" || other.CompareTag("Player"))
        {
            // 通知游戏管理器
            SkyGameManager.Instance?.CollectCoin();
            
            // 销毁金币
            Destroy(gameObject);
        }
    }
}
