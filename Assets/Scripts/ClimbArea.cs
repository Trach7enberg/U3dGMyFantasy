using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 攀爬区域控制,进入climb为1,出去则为0
 */
public class ClimbArea : MonoBehaviour {
    private string tagName = "Luna";
    private Collider2D areaCollider;
    private void Start() {
        areaCollider = this.gameObject.GetComponent<Collider2D>();
    }


    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == tagName) {
            MyLunaController luna = collision.gameObject.GetComponent<MyLunaController>();
            luna.inClimbArea = true;

            if (luna.isClimb) {
                areaCollider.isTrigger = true;

            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == tagName) {
            MyLunaController luna = collision.GetComponent<MyLunaController>();
            luna.inClimbArea = false;
            luna.isClimb = false;
            areaCollider.isTrigger = false;

        }
    }
}
