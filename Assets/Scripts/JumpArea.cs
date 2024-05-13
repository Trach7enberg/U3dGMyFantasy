using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 跳跃区域检测
/// </summary>
public class JumpArea : MonoBehaviour {
    public Transform jumpPointA;
    public Transform jumpPointB;

    private void OnCollisionStay2D(Collision2D collision) {
        MyLunaController luna = collision.transform.GetComponent<MyLunaController>();
        if (luna != null && luna.tag == GameManager.NpcNames.Luna.ToString()) {
            luna.SetJumpArea();
            if (luna.isJump) {
                luna.Jump(jumpPointA, jumpPointB);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        MyLunaController luna = collision.transform.GetComponent<MyLunaController>();
        if (luna != null && luna.tag == GameManager.NpcNames.Luna.ToString()) {
            luna.CloseJumpArea();
        }
    }
}