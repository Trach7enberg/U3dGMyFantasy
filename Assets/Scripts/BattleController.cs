using DG.Tweening;
using System.Collections;
using UnityEngine;

/// <summary>
/// 战斗场景的战斗控制
/// </summary>
public class BattleController : MonoBehaviour {
    public Animator lunaAnimator;
    public Transform lunaTransform;
    public Transform monsterTransform;

    private Vector3 monsterInitPos;
    private Vector3 lunaInitPos;

    public SpriteRenderer monsterRenderer;
    public SpriteRenderer lunaRenderer;

    private string[] animatorParameters = { "isDefend", "MoveVal", "MoveState", };

    // luma普攻伤害
    private float lunaDamage = 20f;

    private float MonsterDamage = 2f;

    private float lunaMoveDuration = 0.5f;
    private float lunaAttackAnimatorDuration = 0.667f;
    private float lunaFadeDuration = 0.333f;
    private float lunaFade = 0.3f;

    private float monsterMoveDuration = 0.5f;
    private float monsterFadeDuration = 0.6f;
    private float monsterFade = 0.3f;

    // 动画序列在状态机里的名字
    private string clipNameAtk = "Attack";

    private string clipNameHurt = "Hurt";

    //public Button attack;
    //public Button defend;
    //public Button skill;
    //public Button recoveryHp;
    //public Button escape;

    // Start is called before the first frame update
    private void Awake() {
        monsterInitPos = monsterTransform.localPosition;
        lunaInitPos = lunaTransform.localPosition;
    }

    // Update is called once per frame
    private void Update() {
    }

    public void Attack() {
        StartCoroutine(PerformAttackLogic());
    }

    /// <summary>
    /// 怪物扣血(暂未完成),渐变恢复
    /// </summary>
    /// <param name="value"></param>
    public void JudgeMonsterHp(float value) {
        monsterRenderer.color = Color.white;
        // 设置怪物的透明度为100%
        monsterRenderer.DOFade(1f, 0);
    }

    public void JudgeLunaHp(float value) {
        lunaRenderer.color = Color.white;
        lunaRenderer.DOFade(1f, 0);
    }

    /// <summary>
    /// 协程,执行luna攻击
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformAttackLogic() {
        UIManager.Instance.ShowBattleUI(false);
        lunaAnimator.SetBool(animatorParameters[2], true);
        lunaAnimator.SetFloat(animatorParameters[1], -1f);

        // luna前移动攻击
        lunaTransform.DOLocalMoveX(monsterTransform.localPosition.x + 1.5f, lunaMoveDuration)
            .OnComplete(() => {
                lunaAnimator.SetBool(animatorParameters[2], false);
                lunaAnimator.SetFloat(animatorParameters[1], 0f);

                // 直接播放attack动画,结束之后自动游回箭头所指的动作,0代表状态机的层级
                lunaAnimator.CrossFade(clipNameAtk, 0);

                //怪物受击渐变动画
                monsterRenderer.color = Color.red;
                monsterRenderer.DOFade(monsterFade, monsterFadeDuration).OnComplete(() => { JudgeMonsterHp(lunaDamage); });
            });

        // 等待移动+攻击完成之后 将luna回移
        yield return new WaitForSeconds(lunaMoveDuration + lunaAttackAnimatorDuration);

        lunaAnimator.SetBool(animatorParameters[2], true);
        lunaAnimator.SetFloat(animatorParameters[1], 1f);
        lunaTransform.DOLocalMove(lunaInitPos, lunaMoveDuration)
            .OnComplete(() => {
                lunaAnimator.SetBool(animatorParameters[2], false);
                lunaAnimator.SetFloat(animatorParameters[1], 0f);
            });
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MonsterAttack());
    }

    /// <summary>
    /// 协程,怪物冲刺攻击
    /// </summary>
    /// <returns></returns>
    private IEnumerator MonsterAttack() {
        monsterTransform.DOLocalMoveX(lunaTransform.localPosition.x - 1.5f, monsterMoveDuration);
        yield return new WaitForSeconds(0.5f);
        monsterTransform.DOLocalMoveX(lunaTransform.localPosition.x - 0.5f, monsterMoveDuration / 2f).OnComplete(() => {
            lunaAnimator.CrossFade(clipNameHurt, 0);
            lunaRenderer.color = Color.red;
            lunaRenderer.DOFade(lunaFade, lunaFadeDuration);
        });

        yield return new WaitForSeconds(monsterMoveDuration / 2f + lunaFadeDuration);
        JudgeLunaHp(MonsterDamage);

        monsterTransform.DOLocalMove(monsterInitPos, monsterMoveDuration).OnComplete(() => {
            UIManager.Instance.ShowBattleUI(true);
        });
        yield return null;
    }
}