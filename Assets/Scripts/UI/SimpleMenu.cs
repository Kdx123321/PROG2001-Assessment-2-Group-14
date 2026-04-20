using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 简单菜单控制器 - 主菜单、场景选择、帮助面板
/// </summary>
public class SimpleMenu : MonoBehaviour
{
    [Header("面板")]
    public GameObject mainPanel;
    public GameObject levelSelectPanel;
    public GameObject helpPanel;

    [Header("主菜单按钮")]
    public Button startButton;
    public Button helpButton;

    [Header("场景选择按钮")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;
    public Button levelBackButton;

    [Header("帮助面板按钮")]
    public Button helpBackButton;

    void Start()
    {
        // 初始化显示主菜单
        ShowMainPanel();

        // 绑定按钮事件
        if (startButton != null)
            startButton.onClick.AddListener(ShowLevelSelect);
        
        if (helpButton != null)
            helpButton.onClick.AddListener(ShowHelpPanel);

        if (level1Button != null)
            level1Button.onClick.AddListener(() => StartGame(0));
        
        if (level2Button != null)
            level2Button.onClick.AddListener(() => StartGame(1));
        
        if (level3Button != null)
            level3Button.onClick.AddListener(() => StartGame(2));
        
        if (level4Button != null)
            level4Button.onClick.AddListener(() => StartGame(3));
        
        if (levelBackButton != null)
            levelBackButton.onClick.AddListener(ShowMainPanel);

        if (helpBackButton != null)
            helpBackButton.onClick.AddListener(ShowMainPanel);
    }

    /// <summary>
    /// 显示主菜单
    /// </summary>
    void ShowMainPanel()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        if (helpPanel != null) helpPanel.SetActive(false);
    }

    /// <summary>
    /// 显示场景选择面板
    /// </summary>
    void ShowLevelSelect()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(true);
        if (helpPanel != null) helpPanel.SetActive(false);
    }

    /// <summary>
    /// 显示帮助面板
    /// </summary>
    void ShowHelpPanel()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        if (helpPanel != null) helpPanel.SetActive(true);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    void StartGame(int levelIndex)
    {
        // 调用GameManager开始游戏
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame(levelIndex);
        }
        else
        {
            Debug.LogWarning("GameManager not found! Loading scene directly.");
            // 如果GameManager不存在，直接加载场景
            string[] sceneNames = { "JungleScene", "RiverScene", "LandScene", "SkyScene" };
            if (levelIndex >= 0 && levelIndex < sceneNames.Length)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNames[levelIndex]);
            }
        }
    }
}
