using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class MyLunaController : MonoBehaviour {
    Rigidbody2D rigibody;
    // 动画状态机
    Animator animator;

    // 当前速度和速度因子
    [SerializeField]
    private float currentSpeed;
    public float speedFactor = 2f;

    Vector2 playerInput;

    // 记录人物当前帧的动画朝向
    private Vector2 towards;

    // 人物是否跳跃
    public bool isJump;
    private float jumpDuration = 0.5f;

    // 人物加速奔跑
    private bool isRun;

    // 人物攀爬
    public bool isClimb;
    public bool inClimbArea;


    

    void Start() {
        //设置帧率
        //Application.targetFrameRate = 30;
        rigibody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        currentSpeed = speedFactor;

        
    }


    void Update() {
        playerInput = GetPlayerInput();

    }

    private void FixedUpdate() {
        Vector2 nextPosition = transform.position;
        nextPosition = playerInput * currentSpeed * Time.fixedDeltaTime + nextPosition;

        UpdateAnimatorState(playerInput);

        rigibody.MovePosition(nextPosition);
    }

    private Vector2 GetPlayerInput() {
        Vector2 p;
        // 用GetAxisRaw而不是GetAxis,因为GetAxis获取输入有一个渐变的过程是慢慢的降下来
        // 2D游戏不需要缓冲,是上就是上是下就是下
        p.x = Input.GetAxisRaw("Horizontal");
        p.y = Input.GetAxisRaw("Vertical");

        // 跳跑爬状态默认是按下切换,而不是持续按下
        // 与运算的作用是 跳跑爬动作同时只能存在一个
        isJump = (isJump) ? !Input.GetKeyDown(KeyCode.Space) : !isRun & !isClimb  & Input.GetKeyDown(KeyCode.Space);
        isRun = (isRun) ? !Input.GetKeyDown(KeyCode.LeftShift) : !isJump & !isClimb & Input.GetKeyDown(KeyCode.LeftShift);
        isClimb = (isClimb) ? !Input.GetKeyDown(KeyCode.LeftControl) : !isJump & !isRun & inClimbArea & Input.GetKeyDown(KeyCode.LeftControl);

        return p;
    }

    /// <summary>
    /// 设置当前luna的刚体组件的simulated属性,可以使luna穿透碰撞体
    /// </summary>
    /// <param name="b"></param>
    public void SetSimulated(bool b) {
        rigibody.simulated = b;
    }

    /// <summary>
    /// luna的跳跃功能
    /// </summary>
    /// <param name="a">跳跃区域对应的A点</param>
    /// <param name="b">跳跃区域对应的B点</param>
    public void Jump(Transform a,Transform b) {
        SetSimulated(false);

        // 分别判断当前距离与A点、B点哪个点的距离远
        float disA = Vector3.Distance(transform.position, a.position);
        float disB = Vector3.Distance(transform.position, b.position);

        // 当前位置如果离A点远,则从当前位置跳跃到A点
        transform.DOMove((disA) > disB ? a.position : b.position, jumpDuration).OnComplete(() => {
            SetSimulated(true);
            isJump = false;
        });


    }

    /// <summary>
    /// 更新Luna动画的状态机
    /// </summary>
    /// <param name="originalInput">获取原生输入</param>
    private void UpdateAnimatorState(Vector2 originalInput) {
        // 玩家输入不为0的情况下就是移动的动画
        if (!Mathf.Approximately(originalInput.x, 0) || !Mathf.Approximately(originalInput.y, 0)) {
            towards.Set(originalInput.x, originalInput.y);
            // 将输入的xy位置归一化(即-1到1之间的范围),长度变为单位1,方向不变
            towards.Normalize();

            // 动作只能触发一种
            if (isJump) {
                animator.SetBool("Jump", true);

            } else if (isClimb) {
                animator.SetBool("Climb", true);
            } else if (isRun) {
                animator.SetBool("Run", true);
                currentSpeed = speedFactor + 1f;
            } else {
                animator.SetBool("Climb", false);
                animator.SetBool("Jump", false);
                animator.SetBool("Run", false);
                currentSpeed = speedFactor;

                rigibody.simulated = true;
            }
        }
        animator.SetFloat("MoveValue", originalInput.magnitude);

        animator.SetFloat("ToX", towards.x);
        animator.SetFloat("ToY", towards.y);
    }

   
}
