using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// 控制主地图上怪物的行为,默认是来回走动
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MonsterController : MonoBehaviour
{

    // t是垂直方向,f是水平方向
    public bool vertical;
    
    // 怪物速度
    public float speed = 2f;

    // 怪物的移动方向,正向还是反向
    private int direction = 1;

    // 怪物改变方向的间隔时间
    private float changeTime = 3f;
    
    // 计时器,怪物每隔n秒就会来回走动
    private float timer;

    private Rigidbody2D rigidbody2d;
    private Animation animator;

    private void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animation>();

        vertical = true;
        timer = changeTime;
    }

    private void FixedUpdate() {
        SetBackAndForth();
    }

    /// <summary>
    /// 控制怪物来回走动
    /// </summary>
    private void SetBackAndForth() {
        Vector2 pos = rigidbody2d.position;
        timer -= Time.fixedDeltaTime;

        // 计时器一到就改变方向
        if(timer < 0) {
            direction = -direction;
            timer = changeTime; 
        }

        if (vertical) {
            pos.y = pos.y + speed * direction * Time.fixedDeltaTime;
        } else {
            pos.x = pos.x + speed * direction * Time.fixedDeltaTime;
        }

        rigidbody2d.MovePosition(pos);
    }
}
