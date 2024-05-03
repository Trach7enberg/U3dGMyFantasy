using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 跳跃区域检测
/// </summary>
public class JumpArea : MonoBehaviour {
    public Transform jumpPointA;
    public Transform jumpPointB;

    private string tagName = "Luna";

    private void OnCollisionStay2D(Collision2D collision) {
        MyLunaController luna = collision.transform.GetComponent<MyLunaController>();
        if (luna != null && luna.tag == tagName) {
            if (luna.isJump) {
                luna.Jump(jumpPointA, jumpPointB);
            }
        }
    }
}