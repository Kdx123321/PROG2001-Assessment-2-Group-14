using UnityEngine;

/// <summary>
/// 音频管理器 - 管理游戏中的所有音效和背景音乐
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("音频源")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("背景音乐")]
    public AudioClip menuMusic;
    public AudioClip jungleMusic;
    public AudioClip riverMusic;
    public AudioClip landMusic;
    public AudioClip skyMusic;

    [Header("音效")]
    public AudioClip coinSound;
    public AudioClip jumpSound;
    public AudioClip slideSound;
    public AudioClip crashSound;
    public AudioClip winSound;
    public AudioClip clickSound;

    [Header("音量设置")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.7f;

    private void Awake()
    {
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

    private void Start()
    {
        // 创建音频源如果没有
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void PlayMusic(string sceneName)
    {
        AudioClip clip = null;
        
        switch (sceneName)
        {
            case "MenuScene":
                clip = menuMusic;
                break;
            case "JungleScene":
                clip = jungleMusic;
                break;
            case "RiverScene":
                clip = riverMusic;
                break;
            case "LandScene":
                clip = landMusic;
                break;
            case "SkyScene":
                clip = skyMusic;
                break;
        }

        if (clip != null && musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySound(string soundName)
    {
        AudioClip clip = null;
        
        switch (soundName)
        {
            case "Coin":
                clip = coinSound;
                break;
            case "Jump":
                clip = jumpSound;
                break;
            case "Slide":
                clip = slideSound;
                break;
            case "Crash":
                clip = crashSound;
                break;
            case "Win":
                clip = winSound;
                break;
            case "Click":
                clip = clickSound;
                break;
        }

        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// 设置音乐音量
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    /// <summary>
    /// 设置音效音量
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }
}
