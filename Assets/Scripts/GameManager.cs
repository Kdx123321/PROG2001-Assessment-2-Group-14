using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏管理器 - 单例模式，管理游戏状态、分数和场景切换
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("游戏状态")]
    public bool isGameActive = false;
    public bool isPaused = false;

    [Header("分数")]
    public int coinCount = 0;
    public int score = 0;
    public float distance = 0f;

    [Header("场景名称")]
    public string menuSceneName = "MenuScene";
    public string[] gameScenes = { "JungleScene", "RiverScene", "LandScene", "SkyScene" };

    private void Awake()
    {
        // 单例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isGameActive && !isPaused)
        {
            UpdateScore();
        }
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < gameScenes.Length)
        {
            SceneManager.LoadScene(gameScenes[sceneIndex]);
            ResetGameData();
            isGameActive = true;
        }
    }

    /// <summary>
    /// 加载特定场景
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void ReturnToMenu()
    {
        isGameActive = false;
        SceneManager.LoadScene(menuSceneName);
    }

    /// <summary>
    /// 重置游戏数据
    /// </summary>
    void ResetGameData()
    {
        coinCount = 0;
        score = 0;
        distance = 0f;
        isPaused = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 更新分数
    /// </summary>
    void UpdateScore()
    {
        score += Mathf.RoundToInt(Time.deltaTime * 10);
        UIManager.Instance?.UpdateScore(score);
    }

    /// <summary>
    /// 收集金币
    /// </summary>
    public void CollectCoin()
    {
        coinCount++;
        score += 10;
        UIManager.Instance?.UpdateCoinCount(coinCount);
        AudioManager.Instance?.PlaySound("Coin");
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public void GameOver()
    {
        isGameActive = false;
        UIManager.Instance?.ShowGameOverPanel();
    }

    /// <summary>
    /// 游戏胜利
    /// </summary>
    public void GameWin()
    {
        isGameActive = false;
        UIManager.Instance?.ShowWinPanel();
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        UIManager.Instance?.ShowPausePanel();
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        UIManager.Instance?.HidePausePanel();
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    /// <summary>
    /// 获取当前场景索引
    /// </summary>
    public int GetCurrentSceneIndex()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        for (int i = 0; i < gameScenes.Length; i++)
        {
            if (gameScenes[i] == currentScene)
                return i;
        }
        return -1;
    }
}
