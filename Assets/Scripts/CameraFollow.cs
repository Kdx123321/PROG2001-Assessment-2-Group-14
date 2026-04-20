using UnityEngine;

/// <summary>
/// 相机跟随脚本 - 让相机平滑跟随玩家
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("跟随目标")]
    public Transform target;

    [Header("位置偏移")]
    public Vector3 offset = new Vector3(0, 3, -5);

    [Header("跟随速度")]
    public float smoothSpeed = 5f;

    [Header("是否看向目标")]
    public bool lookAtTarget = true;

    void LateUpdate()
    {
        if (target == null) return;

        // 计算目标位置
        Vector3 desiredPosition = target.position + offset;
        
        // 平滑插值到目标位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // 让相机看向玩家（可选）
        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }
}
