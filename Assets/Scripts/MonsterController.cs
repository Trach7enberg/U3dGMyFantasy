using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// ��������ͼ�Ϲ������Ϊ,Ĭ���������߶�
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MonsterController : MonoBehaviour
{

    // t�Ǵ�ֱ����,f��ˮƽ����
    public bool vertical;
    
    // �����ٶ�
    public float speed = 2f;

    // ������ƶ�����,�����Ƿ���
    private int direction = 1;

    // ����ı䷽��ļ��ʱ��
    private float changeTime = 3f;
    
    // ��ʱ��,����ÿ��n��ͻ������߶�
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
    /// ���ƹ��������߶�
    /// </summary>
    private void SetBackAndForth() {
        Vector2 pos = rigidbody2d.position;
        timer -= Time.fixedDeltaTime;

        // ��ʱ��һ���͸ı䷽��
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
