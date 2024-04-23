using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 战斗场景的战斗控制
/// </summary>
public class BattleController : MonoBehaviour
{
    public Animator lunaAnimator;
    public Transform lunaTransform;
    public Transform monsterTransform;

    private Vector3 monsterInitPos;
    private Vector3 lunaInitPos;

    public SpriteRenderer monsterRenderer;

    private string[] animatorParameters = { "isDefend", "MoveVal", "MoveState", };

    // luma普攻伤害
    private float lunaDamage = 20f;

    private float moveDuration = 0.4f;
    private float monsterFadeDuration = 0.2f;

    //public Button attack;
    //public Button defend;
    //public Button skill;
    //public Button recoveryHp;
    //public Button escape;

    // Start is called before the first frame update
    void Awake()
    {
        monsterInitPos = monsterTransform.localPosition;
        lunaInitPos = lunaTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack() {
        StartCoroutine(PerformAttackLogic());
    }

    /// <summary>
    /// 怪物扣血(暂未完成),渐变恢复
    /// </summary>
    /// <param name="value"></param>
    public void JudgeMonsterHP(float value) {
        monsterRenderer.color = Color.white;
        monsterRenderer.DOFade(1f, monsterFadeDuration);
    }

    IEnumerator PerformAttackLogic() {
        UIManager.Instance.ShowBattleUI(false);
        lunaAnimator.SetBool(animatorParameters[2], true);
        lunaAnimator.SetFloat(animatorParameters[1], -1f);

        lunaTransform.DOLocalMove(monsterInitPos + new Vector3(1.5f, 0f, 0), moveDuration)
            .OnComplete(()=>{
                lunaAnimator.SetBool(animatorParameters[2], false);
                lunaAnimator.SetFloat(animatorParameters[1], 0f);
                
                // 直接播放attack动画,结束之后自动游回箭头所指的动作,0代表状态机的层级
                lunaAnimator.CrossFade("LunaAttack", 0);
                //怪物受击渐变动画
                monsterRenderer.color = Color.red;
                monsterRenderer.DOFade(0.4f, monsterFadeDuration).OnComplete(() => { JudgeMonsterHP(lunaDamage); });

            });
        yield return null;
    }
}
