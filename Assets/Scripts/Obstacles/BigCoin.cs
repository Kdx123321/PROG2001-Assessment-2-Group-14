using UnityEngine;

/// <summary>
/// 大金币脚本 - 关卡终点的通关金币
/// 放置在关卡尽头的大金币上
/// </summary>
public class BigCoin : MonoBehaviour
{
    [Header("旋转设置")]
    public float rotationSpeed = 50f;
    
    [Header("缩放效果")]
    public bool usePulse = true;
    public float pulseSpeed = 2f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    [Header("粒子效果")]
    public ParticleSystem sparkleEffect;

    void Start()
    {
        // 启动粒子效果
        if (sparkleEffect != null)
        {
            sparkleEffect.Play();
        }
    }

    void Update()
    {
        // 旋转大金币
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        
        // 脉动效果
        if (usePulse)
        {
            float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * pulseSpeed) + 1) / 2);
            transform.localScale = Vector3.one * scale;
        }
    }

    /// <summary>
    /// 触发胜利
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 销毁时创建庆祝效果
            if (sparkleEffect != null)
            {
                sparkleEffect.transform.SetParent(null);
                sparkleEffect.Play();
                Destroy(sparkleEffect.gameObject, 2f);
            }
            
            Destroy(gameObject);
        }
    }
}
