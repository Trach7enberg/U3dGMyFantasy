using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class MyLunaController : MonoBehaviour {
    private Rigidbody2D rigibody;

    // 动画状态机
    private Animator animator;

    //luna对象下的第一个子类,即LunaSprite
    private Transform lunaLocalTransform;

    // 当前速度和速度因子
    [SerializeField]
    private float currentSpeed;

    public float speedFactor = 2f;

    private Vector2 playerInput;

    // 记录人物当前帧的动画朝向
    private Vector2 towards;

    // 人物是否跳跃
    public bool isJump;

    private float jumpDuration = 0.5f;

    // 完成一个跳跃逼真效果的时间
    private float jumpRealityDuration = 0.25f;

    // 跳跃逼真效果的幅度
    private float jumpRealitySize = 1f;

    private float lunaLocalPositionYOriginal;

    // 人物加速奔跑
    private bool isRun;

    // 人物攀爬
    public bool isClimb;

    public bool inClimbArea;

    private float TouchTheDogDuration = 0.5f;
    private float LookAtTheDogDuration = 1f;

    private void Start() {
        //设置帧率
        //Application.targetFrameRate = 30;
        rigibody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        lunaLocalTransform = transform.GetChild(0);
        lunaLocalPositionYOriginal = lunaLocalTransform.localPosition.y;
        currentSpeed = speedFactor;
    }

    private void Update() {
        playerInput = GetPlayerInput();
    }

    private void FixedUpdate() {
        Vector2 nextPosition = transform.position;
        nextPosition = playerInput * currentSpeed * Time.fixedDeltaTime + nextPosition;

        if (GameManager.Instance.CanControlLuna) {
            UpdateAnimatorState(playerInput);

            rigibody.MovePosition(nextPosition);
        }
    }

    private Vector2 GetPlayerInput() {
        Vector2 p;
        // 用GetAxisRaw而不是GetAxis,因为GetAxis获取输入有一个渐变的过程是慢慢的降下来
        // 2D游戏不需要缓冲,是上就是上是下就是下
        p.x = Input.GetAxisRaw("Horizontal");
        p.y = Input.GetAxisRaw("Vertical");

        // 跳跑爬状态默认是按下切换,而不是持续按下
        // 与运算的作用是 跳跑爬动作同时只能存在一个
        isJump = (isJump) ? !Input.GetKeyDown(KeyCode.Space) : !isRun & !isClimb & Input.GetKeyDown(KeyCode.Space);
        isRun = (isRun) ? !Input.GetKeyDown(KeyCode.LeftShift) : !isJump & !isClimb & Input.GetKeyDown(KeyCode.LeftShift);
        isClimb = (isClimb) ? !Input.GetKeyDown(KeyCode.LeftControl) : !isJump & !isRun & inClimbArea & Input.GetKeyDown(KeyCode.LeftControl);
        if (Input.GetKeyDown(KeyCode.F)) {
            DoSomething();
        }
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
    public void Jump(Transform a, Transform b) {
        SetSimulated(false);

        // 分别判断当前距离与A点、B点哪个点的距离远
        float disA = Vector3.Distance(transform.position, a.position);
        float disB = Vector3.Distance(transform.position, b.position);

        // 当前位置如果离A点远,则从当前位置跳跃到A点
        transform.DOMove((disA) > disB ? a.position : b.position, jumpDuration).SetEase(Ease.Linear).OnComplete(
            () => {
                //跳跃结束时luna的刚体模拟要恢复否则会卡住不动
                SetSimulated(true);
                isJump = false;
            });

        // 移动luna下的lunaSprite,让跳跃更加真实,动作分成两段,第一段是增加Y距离,第二段是恢复默认Y值
        // 可以用DOTween Sequence 队列来保存这两个动作,然后自动按顺序播放
        // SetEase 是设置动画播放的速度,InOutSine 指的是sin函数的正周期,即动作从0开始加速后半段归0,非常符合跳跃的效果
        lunaLocalTransform.DOLocalMoveY(
                lunaLocalPositionYOriginal + jumpRealitySize, jumpRealityDuration).SetEase(Ease.InOutSine)
            .OnComplete(
                () => {
                    lunaLocalTransform.DOLocalMoveY(lunaLocalPositionYOriginal, jumpRealityDuration).SetEase(Ease.InOutSine);
                }
            );
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

            // 动作只能触发一种(有动画状态的动作)
            if (isJump) {
                animator.SetBool(GameManager.AnimatorParameters.Jump.ToString(), true);
            } else if (isClimb) {
                animator.SetBool(GameManager.AnimatorParameters.Climb.ToString(), true);
            } else if (isRun) {
                animator.SetBool(GameManager.AnimatorParameters.Run.ToString(), true);
                currentSpeed = speedFactor + 1f;
            } else {
                animator.SetBool(GameManager.AnimatorParameters.Climb.ToString(), false);
                animator.SetBool(GameManager.AnimatorParameters.Jump.ToString(), false);
                animator.SetBool(GameManager.AnimatorParameters.Run.ToString(), false);
                currentSpeed = speedFactor;
            }
        }
        animator.SetFloat(GameManager.AnimatorParameters.MoveValue.ToString(), originalInput.magnitude);

        animator.SetFloat(GameManager.AnimatorParameters.ToX.ToString(), towards.x);
        animator.SetFloat(GameManager.AnimatorParameters.ToY.ToString(), towards.y);
    }

    /// <summary>
    /// 与NPC互动对话、领取任务、完成任务
    /// </summary>
    public void DoSomething() {
        // 以某个刚体为半径的自交球,有东西在这个半径内 就代表检测成功,第一个参数是以谁为中心点,第二个参数是检测半径,第三参数是检测的是哪个层级的游戏物体
        Collider2D c = Physics2D.OverlapCircle(rigibody.position, 0.5f, LayerMask.GetMask(UiManager.GameLayerMask.Npc.ToString()));

        if (c != null && Enum.IsDefined(typeof(GameManager.NpcNames), c.name)) {
            switch ((GameManager.NpcNames)Enum.Parse(typeof(GameManager.NpcNames), c.name)) {
                // 与Nala互动
                case GameManager.NpcNames.Nala:
                    Animator nalaAnimator = c.GetComponentInParent<NpcDialog>().NalaAnimator;
                    // 播放对应NPC触发对话的动画
                    nalaAnimator.CrossFade(GameManager.AnimatorMotionName.TalkLaugh.ToString(), 0);
                    // 拿到碰撞对象NPC下的父类然后播放对话
                    c.GetComponentInParent<NpcDialog>().DisplayDialog();
                    break;

                // 与狗子互动
                case GameManager.NpcNames.Dog:
                    Animator dogAnimator = c.GetComponentInParent<NpcDialog>().DogAnimator;
                    // 已领取任务并且当前任务的名字是摸狗子时
                    if (MissionsManager.Instance.Missions[GameManager.Instance.MissionsIndex].IsClaimed
                        && MissionsManager.Instance.Missions[GameManager.Instance.MissionsIndex].Name
                        == MissionsManager.MissionsName.PetTheDog) {
                        StartCoroutine(PetTheDogAnimation(dogAnimator, true));

                        MissionsManager.Instance.Missions[GameManager.Instance.MissionsIndex].IsDone = true;
                        GameManager.Instance.MissionsIndex++;
                        MissionsManager.Instance.DialogIndex = 0;
                    } else {
                        StartCoroutine(PetTheDogAnimation(dogAnimator, false));
                    }
                    // 完成任务后狗子可以随便摸,,TODO
                    break;
            }
        }
    }

    /// <summary>
    /// 摸狗子动画
    /// </summary>
    /// <param name="canPet">能摸和不能摸都播放相应动画</param>
    /// <returns></returns>
    private IEnumerator PetTheDogAnimation(Animator DogAnimator, bool canPet) {
        if (canPet) {
            // 摸狗的时候可以直接瞬移luna的位置 TODO
            animator.CrossFade(GameManager.AnimatorMotionName.TouchTheDog.ToString(), 0);
            DogAnimator.CrossFade(GameManager.AnimatorMotionName.DogBeHappy.ToString(), 0);
        } else {
            animator.CrossFade(GameManager.AnimatorMotionName.LookTheDog.ToString(), 0);
            DogAnimator.CrossFade(GameManager.AnimatorMotionName.DogBark.ToString(), 0);
        }
        yield return 0;
    }
}