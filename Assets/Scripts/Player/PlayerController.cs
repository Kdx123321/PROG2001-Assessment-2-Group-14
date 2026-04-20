using UnityEngine;

/// <summary>
/// 玩家控制器 - 处理玩家的移动、跳跃、下滑和轨道切换
/// 这是游戏的核心控制脚本，所有场景共用
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("轨道设置")]
    public float laneDistance = 3f;  // 轨道间距
    public float laneSwitchSpeed = 10f;  // 换道速度
    private int currentLane = 1;  // 当前轨道 0=左, 1=中, 2=右
    private int targetLane = 1;   // 目标轨道

    [Header("移动设置")]
    public float forwardSpeed = 10f;  // 前进速度
    public float speedIncreaseRate = 0.1f;  // 速度增加率

    [Header("跳跃设置")]
    public float jumpForce = 8f;  // 跳跃力度
    public float gravity = 20f;   // 重力
    private bool isGrounded = true;
    private float verticalVelocity = 0f;

    [Header("下滑设置")]
    public float slideDuration = 1f;  // 下滑持续时间
    public float slideHeight = 0.5f;  // 下滑时的高度缩放
    private bool isSliding = false;
    private float slideTimer = 0f;
    private Vector3 originalScale;

    [Header("组件引用")]
    public CharacterController controller;
    public Animator animator;

    private Vector3 targetPosition;
    private bool isGameOver = false;
    private bool isGameWon = false;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponent<Animator>();
        
        originalScale = transform.localScale;
        targetPosition = transform.position;
    }

    void Update()
    {
        if (isGameOver || isGameWon) return;

        HandleInput();
        MoveForward();
        HandleLaneSwitch();
        HandleJump();
        HandleSlide();
    }

    /// <summary>
    /// 处理玩家输入 (A/D 或 左右箭头切换轨道，空格跳跃，S或下箭头下滑)
    /// </summary>
    void HandleInput()
    {
        // 左移 (A 或 左箭头)
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (targetLane > 0)
            {
                targetLane--;
                PlayAnimation("LeftMove");
            }
        }
        // 右移 (D 或 右箭头)
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (targetLane < 2)
            {
                targetLane++;
                PlayAnimation("RightMove");
            }
        }

        // 跳跃 (空格)
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded && !isSliding)
        {
            Jump();
        }

        // 下滑 (S 或 下箭头)
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding && isGrounded)
        {
            StartSlide();
        }
    }

    /// <summary>
    /// 向前移动并逐渐增加速度
    /// </summary>
    void MoveForward()
    {
        forwardSpeed += speedIncreaseRate * Time.deltaTime;
        
        Vector3 forwardMove = Vector3.forward * forwardSpeed * Time.deltaTime;
        controller.Move(forwardMove);
    }

    /// <summary>
    /// 处理轨道间的平滑切换
    /// </summary>
    void HandleLaneSwitch()
    {
        // 计算目标X位置
        float targetX = (targetLane - 1) * laneDistance;
        
        // 平滑过渡到目标位置
        Vector3 currentPosition = transform.position;
        float newX = Mathf.Lerp(currentPosition.x, targetX, laneSwitchSpeed * Time.deltaTime);
        
        Vector3 move = new Vector3(newX - currentPosition.x, 0, 0);
        controller.Move(move);
    }

    /// <summary>
    /// 处理跳跃逻辑
    /// </summary>
    void Jump()
    {
        verticalVelocity = jumpForce;
        isGrounded = false;
        PlayAnimation("Jump");
        
        // 触发跳跃音效
        AudioManager.Instance?.PlaySound("Jump");
    }

    /// <summary>
    /// 处理跳跃物理
    /// </summary>
    void HandleJump()
    {
        // 应用重力
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // 检测是否着地
        if (transform.position.y <= 0.1f && !isGrounded)
        {
            verticalVelocity = 0;
            isGrounded = true;
            PlayAnimation("Run");
        }

        // 应用垂直移动
        Vector3 verticalMove = new Vector3(0, verticalVelocity * Time.deltaTime, 0);
        controller.Move(verticalMove);
    }

    /// <summary>
    /// 开始下滑
    /// </summary>
    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        
        // 缩小高度
        transform.localScale = new Vector3(originalScale.x, originalScale.y * slideHeight, originalScale.z);
        PlayAnimation("Slide");
        
        // 触发下滑音效
        AudioManager.Instance?.PlaySound("Slide");
    }

    /// <summary>
    /// 处理下滑逻辑
    /// </summary>
    void HandleSlide()
    {
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            
            if (slideTimer <= 0)
            {
                EndSlide();
            }
        }
    }

    /// <summary>
    /// 结束下滑
    /// </summary>
    void EndSlide()
    {
        isSliding = false;
        transform.localScale = originalScale;
        PlayAnimation("Run");
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    void PlayAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.SetTrigger(animationName);
        }
    }

    /// <summary>
    /// 碰撞检测 - 碰到障碍物
    /// </summary>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
        else if (hit.gameObject.CompareTag("BigCoin"))
        {
            GameWin();
        }
    }

    /// <summary>
    /// 触发器检测 - 收集金币
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.Instance?.CollectCoin();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BigCoin"))
        {
            GameWin();
        }
        else if (other.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        PlayAnimation("Hit");
        AudioManager.Instance?.PlaySound("Crash");
        GameManager.Instance?.GameOver();
    }

    /// <summary>
    /// 游戏胜利
    /// </summary>
    void GameWin()
    {
        if (isGameWon) return;
        isGameWon = true;
        PlayAnimation("Win");
        AudioManager.Instance?.PlaySound("Win");
        GameManager.Instance?.GameWin();
    }

    /// <summary>
    /// 获取当前是否正在下滑
    /// </summary>
    public bool IsSliding()
    {
        return isSliding;
    }

    /// <summary>
    /// 获取当前是否在地面上
    /// </summary>
    public bool IsGrounded()
    {
        return isGrounded;
    }
}
