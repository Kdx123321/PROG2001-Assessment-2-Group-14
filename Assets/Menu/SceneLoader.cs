using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景加载器 - 用于在场景之间过渡
/// 团队共同使用
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [Header("加载设置")]
    public float transitionDelay = 0.5f;
    public GameObject loadingScreen;

    /// <summary>
    /// 加载场景
    /// </summary>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    System.Collections.IEnumerator LoadSceneAsync(string sceneName)
    {
        // 显示加载画面
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        // 等待一小段时间
        yield return new WaitForSeconds(transitionDelay);

        // 异步加载场景
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            // 可以在这里更新加载进度条
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
    }

    /// <summary>
    /// 重新加载当前场景
    /// </summary>
    public void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
}
