using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI管理器 - 管理所有UI元素的显示和更新
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("游戏内UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public GameObject gameplayPanel;

    [Header("游戏结束面板")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalCoinText;
    public Button restartButton;
    public Button menuButton;

    [Header("胜利面板")]
    public GameObject winPanel;
    public TextMeshProUGUI winScoreText;
    public TextMeshProUGUI winCoinText;
    public Button nextLevelButton;
    public Button winMenuButton;

    [Header("暂停面板")]
    public GameObject pausePanel;
    public Button resumeButton;
    public Button pauseMenuButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeUI();
        SetupButtons();
    }

    /// <summary>
    /// 初始化UI
    /// </summary>
    void InitializeUI()
    {
        // 隐藏所有面板
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);
        if (gameplayPanel) gameplayPanel.SetActive(true);

        UpdateScore(0);
        UpdateCoinCount(0);
    }

    /// <summary>
    /// 设置按钮事件
    /// </summary>
    void SetupButtons()
    {
        if (restartButton)
            restartButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound("Click");
                GameManager.Instance?.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            });

        if (menuButton)
            menuButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound("Click");
                GameManager.Instance?.ReturnToMenu();
            });

        if (nextLevelButton)
            nextLevelButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound("Click");
                int nextScene = GameManager.Instance.GetCurrentSceneIndex() + 1;
                if (nextScene < 4)
                    GameManager.Instance.StartGame(nextScene);
                else
                    GameManager.Instance.ReturnToMenu();
            });

        if (winMenuButton)
            winMenuButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound("Click");
                GameManager.Instance?.ReturnToMenu();
            });

        if (resumeButton)
            resumeButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound("Click");
                GameManager.Instance?.ResumeGame();
            });

        if (pauseMenuButton)
            pauseMenuButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound("Click");
                GameManager.Instance?.ReturnToMenu();
            });
    }

    /// <summary>
    /// 更新分数显示
    /// </summary>
    public void UpdateScore(int score)
    {
        if (scoreText)
            scoreText.text = "Score: " + score.ToString();
    }

    /// <summary>
    /// 更新金币显示
    /// </summary>
    public void UpdateCoinCount(int count)
    {
        if (coinText)
            coinText.text = "Coins: " + count.ToString();
    }

    /// <summary>
    /// 显示游戏结束面板
    /// </summary>
    public void ShowGameOverPanel()
    {
        if (gameplayPanel) gameplayPanel.SetActive(false);
        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText)
                finalScoreText.text = "Final Score: " + GameManager.Instance.score.ToString();
            if (finalCoinText)
                finalCoinText.text = "Coins: " + GameManager.Instance.coinCount.ToString();
        }
    }

    /// <summary>
    /// 显示胜利面板
    /// </summary>
    public void ShowWinPanel()
    {
        if (gameplayPanel) gameplayPanel.SetActive(false);
        if (winPanel)
        {
            winPanel.SetActive(true);
            if (winScoreText)
                winScoreText.text = "Final Score: " + GameManager.Instance.score.ToString();
            if (winCoinText)
                winCoinText.text = "Coins: " + GameManager.Instance.coinCount.ToString();
            
            // 检查是否是最后一个场景
            int currentScene = GameManager.Instance.GetCurrentSceneIndex();
            if (nextLevelButton)
            {
                if (currentScene >= 3)
                {
                    TextMeshProUGUI buttonText = nextLevelButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText) buttonText.text = "Back to Menu";
                }
            }
        }
    }

    /// <summary>
    /// 显示暂停面板
    /// </summary>
    public void ShowPausePanel()
    {
        if (pausePanel) pausePanel.SetActive(true);
    }

    /// <summary>
    /// 隐藏暂停面板
    /// </summary>
    public void HidePausePanel()
    {
        if (pausePanel) pausePanel.SetActive(false);
    }
}
