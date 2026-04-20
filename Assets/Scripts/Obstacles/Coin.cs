using UnityEngine;

/// <summary>
/// 金币脚本 - 可以旋转金币并添加浮动效果
/// 放置在金币预制体上
/// </summary>
public class Coin : MonoBehaviour
{
    [Header("旋转设置")]
    public float rotationSpeed = 100f;
    
    [Header("浮动设置")]
    public bool useFloat = true;
    public float floatAmplitude = 0.2f;
    public float floatSpeed = 2f;
    
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 旋转金币
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        
        // 浮动效果
        if (useFloat)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        }
    }
}
