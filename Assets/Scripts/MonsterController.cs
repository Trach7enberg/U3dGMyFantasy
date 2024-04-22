using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// ��������ͼ�Ϲ������Ϊ,Ĭ���������߶�
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MonsterController : MonoBehaviour
{

    // t�Ǵ�ֱ����,f��ˮƽ����
    public bool isVertical;
    
    // �����ٶ�
    public float speed = 2f;

    // ������ƶ�����,�����Ƿ���
    private int direction = 1;

    // ����ı䷽��ļ��ʱ��
    private float changeTime = 3f;
    
    // ��ʱ��,����ÿ��n��ͻ������߶�
    private float timer;

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private string[] AnimatorParameters = { "ToX","ToY"};
    private Vector2 lastPosition ;
    private Vector2 nowPosition;

    private void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastPosition = rigidbody2d.position;
        nowPosition = rigidbody2d.position;

        isVertical = false;
        timer = changeTime;
    }

    private void FixedUpdate() {
        Move();
        UpdateAnimatorState();
    }
    private void Update() {
        
    }

    /// <summary>
    /// ���ƹ��������߶�
    /// </summary>
    private void Move() {
        lastPosition = rigidbody2d.position;
        Vector2 pos = rigidbody2d.position;
        timer -= Time.fixedDeltaTime;

        // ��ʱ��һ���͸ı䷽��
        if(timer < 0) {
            direction = -direction;
            timer = changeTime; 
        }

        // ����ˮƽ���ߴ�ֱ���������ĸ���ת��
        if (isVertical) {
            pos.y = pos.y + speed * direction * Time.fixedDeltaTime;
        } else {
            pos.x = pos.x + speed * direction * Time.fixedDeltaTime;
        }
        
        rigidbody2d.MovePosition(pos);
        nowPosition = pos;
        
    }

    /// <summary>
    /// ����monster����״̬��,Ŀǰmonsterֻ��򵥵������ƶ�
    /// </summary>
    private void UpdateAnimatorState() {
        lastPosition.Normalize();
        nowPosition.Normalize();
        
        
        Debug.Log( nowPosition.x);

        animator.SetFloat(AnimatorParameters[0],(isVertical) ? 0: direction);
        animator.SetFloat(AnimatorParameters[1], (isVertical) ? direction : 0);
    }
}
