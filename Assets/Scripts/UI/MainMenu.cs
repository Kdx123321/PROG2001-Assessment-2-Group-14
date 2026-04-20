using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 主菜单控制器 - 团队共同开发
/// 管理主菜单的所有功能
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("菜单面板")]
    public GameObject mainPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("主菜单按钮")]
    public Button startButton;
    public Button levelSelectButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button quitButton;

    [Header("关卡选择")]
    public Button[] levelButtons; // 4个关卡按钮
    public Button levelBackButton;

    [Header("设置面板")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Button settingsBackButton;

    [Header("制作人员")]
    public Button creditsBackButton;

    [Header("标题")]
    public TextMeshProUGUI titleText;
    public GameObject titleAnimation;

    void Start()
    {
        InitializeMenu();
        SetupButtons();
        PlayTitleAnimation();
        
        // 播放菜单音乐
        AudioManager.Instance?.PlayMusic("MenuScene");
    }

    /// <summary>
    /// 初始化菜单状态
    /// </summary>
    void InitializeMenu()
    {
        // 只显示主面板
        mainPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);

        // 加载音量设置
        if (musicSlider != null)
        {
            musicSlider.value = AudioManager.Instance != null ? AudioManager.Instance.musicVolume : 0.5f;
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = AudioManager.Instance != null ? AudioManager.Instance.sfxVolume : 0.7f;
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
    }

    /// <summary>
    /// 设置按钮事件
    /// </summary>
    void SetupButtons()
    {
        // 主菜单按钮
        if (startButton)
            startButton.onClick.AddListener(() => {
                PlayClickSound();
                StartGame(0); // 从第一关开始
            });

        if (levelSelectButton)
            levelSelectButton.onClick.AddListener(() => {
                PlayClickSound();
                ShowLevelSelect();
            });

        if (settingsButton)
            settingsButton.onClick.AddListener(() => {
                PlayClickSound();
                ShowSettings();
            });

        if (creditsButton)
            creditsButton.onClick.AddListener(() => {
                PlayClickSound();
                ShowCredits();
            });

        if (quitButton)
            quitButton.onClick.AddListener(() => {
                PlayClickSound();
                QuitGame();
            });

        // 关卡选择按钮
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i; // 捕获索引
            if (levelButtons[i])
            {
                levelButtons[i].onClick.AddListener(() => {
                    PlayClickSound();
                    StartGame(levelIndex);
                });
            }
        }

        if (levelBackButton)
            levelBackButton.onClick.AddListener(() => {
                PlayClickSound();
                ShowMainPanel();
            });

        // 设置面板按钮
        if (settingsBackButton)
            settingsBackButton.onClick.AddListener(() => {
                PlayClickSound();
                ShowMainPanel();
            });

        // 制作人员面板按钮
        if (creditsBackButton)
            creditsBackButton.onClick.AddListener(() => {
                PlayClickSound();
                ShowMainPanel();
            });
    }

    /// <summary>
    /// 播放标题动画
    /// </summary>
    void PlayTitleAnimation()
    {
        // 可以使用动画系统或简单的代码动画
        if (titleText != null)
        {
            // 简单的缩放动画 - 使用 Unity 原生动画
            // 如需使用 LeanTween，请先导入 LeanTween 插件
            // LeanTween.scale(titleText.gameObject, Vector3.one * 1.1f, 1f)
            //     .setLoopPingPong()
            //     .setEaseInOutSine();
        }
    }

    /// <summary>
    /// 播放点击音效
    /// </summary>
    void PlayClickSound()
    {
        AudioManager.Instance?.PlaySound("Click");
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    void StartGame(int levelIndex)
    {
        GameManager.Instance?.StartGame(levelIndex);
    }

    /// <summary>
    /// 显示主面板
    /// </summary>
    void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    /// <summary>
    /// 显示关卡选择
    /// </summary>
    void ShowLevelSelect()
    {
        mainPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    /// <summary>
    /// 显示设置面板
    /// </summary>
    void ShowSettings()
    {
        mainPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    /// <summary>
    /// 显示制作人员
    /// </summary>
    void ShowCredits()
    {
        mainPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    /// <summary>
    /// 音乐音量变化
    /// </summary>
    void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance?.SetMusicVolume(value);
    }

    /// <summary>
    /// 音效音量变化
    /// </summary>
    void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance?.SetSFXVolume(value);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    void QuitGame()
    {
        GameManager.Instance?.QuitGame();
    }
}
