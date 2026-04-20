#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// 自动创建UI功能已禁用
/// 请手动在场景中配置UI元素
/// </summary>
public static class UIAutoSetup
{
    [MenuItem("Tools/自动配置UIManager (已禁用)")]
    public static void SetupUI()
    {
        Debug.Log("⚠️ 自动配置UI功能已禁用。请手动在场景中配置UI元素。");
        Debug.Log("提示：在Unity中手动创建Canvas和UI元素，然后拖到UIManager脚本中。");
    }
}
#endif
