using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// 控制主地图上怪物的行为,默认是来回走动
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MonsterController : MonoBehaviour {

    // t是垂直方向,f是水平方向
    public bool isVertical;

    // 怪物速度
    public float speed = 2f;

    // 怪物的移动方向,正向还是反向
    private int direction = 1;

    // 怪物改变方向的间隔时间
    private float changeTime = 3f;

    // 计时器,怪物每隔n秒就会来回走动
    private float timer;

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private string[] AnimatorParameters = { "ToX", "ToY" };
    private string tagLuna = "Luna";

    private Vector2 lastPosition;
    private Vector2 nowPosition;

    private void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastPosition = rigidbody2d.position;
        nowPosition = rigidbody2d.position;

        timer = changeTime;
    }

    private void FixedUpdate() {
        Move();
        UpdateAnimatorState();
    }

    private void Update() {
    }

    /// <summary>
    /// 控制怪物来回走动
    /// </summary>
    private void Move() {
        lastPosition = rigidbody2d.position;
        Vector2 pos = rigidbody2d.position;
        timer -= Time.fixedDeltaTime;

        // 计时器一到就改变方向
        if (timer < 0) {
            direction = -direction;
            timer = changeTime;
        }

        // 根据水平或者垂直方向来更改刚体转向
        if (isVertical) {
            pos.y = pos.y + speed * direction * Time.fixedDeltaTime;
        } else {
            pos.x = pos.x + speed * direction * Time.fixedDeltaTime;
        }

        rigidbody2d.MovePosition(pos);
        nowPosition = pos;
    }

    /// <summary>
    /// 更新monster动画状态机,目前monster只会简单的上下移动
    /// </summary>
    private void UpdateAnimatorState() {
        lastPosition.Normalize();
        nowPosition.Normalize();

        animator.SetFloat(AnimatorParameters[0], (isVertical) ? 0 : direction);
        animator.SetFloat(AnimatorParameters[1], (isVertical) ? direction : 0);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == tagLuna) {
            GameManager.Instance.ShowBattleGround();
            UIManager.Instance.ShowBattleUI();
        }
    }
}