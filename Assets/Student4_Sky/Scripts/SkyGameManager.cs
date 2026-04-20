using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyGameManager : MonoBehaviour
{
    public static SkyGameManager Instance;
    
    [Header("金币总数")]
    public int totalCoins = 10;
    
    [Header("当前金币数")]
    private int collectedCoins = 0;
    
    [Header("游戏是否结束")]
    private bool gameEnded = false;

    void Awake()
    {
        // 单例模式
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        collectedCoins = 0;
        gameEnded = false;
        Debug.Log("游戏开始！需要收集 " + totalCoins + " 个金币");
    }

    public void CollectCoin()
    {
        if (gameEnded) return;
        
        collectedCoins++;
        Debug.Log("收集金币！当前: " + collectedCoins + "/" + totalCoins);
        
        // 检查是否收集完所有金币
        if (collectedCoins >= totalCoins)
        {
            GameWin();
        }
    }

    void GameWin()
    {
        gameEnded = true;
        Debug.Log("🎉 恭喜！你收集了所有金币，游戏胜利！");
        
        // 可以在这里添加胜利画面、返回主菜单等
        // 例如：SceneManager.LoadScene("Menu");
    }

    public int GetCollectedCoins()
    {
        return collectedCoins;
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }
}
