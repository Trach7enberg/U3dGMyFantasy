using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÌøÔ¾ÇøÓò¼ì²â
/// </summary>
public class JumpArea : MonoBehaviour
{
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
