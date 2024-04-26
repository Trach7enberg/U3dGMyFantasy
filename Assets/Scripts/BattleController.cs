using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 战斗场景的战斗控制
/// </summary>
public class BattleController : MonoBehaviour {
    public Animator lunaAnimator;
    public Transform lunaTransform;
    public Transform monsterTransform;
    public GameObject SkillEffect;
    private GameObject skillEffectCopy;

    private Vector3 monsterInitPos;
    private Vector3 lunaInitPos;

    public SpriteRenderer monsterRenderer;
    public SpriteRenderer lunaRenderer;

    private string[] animatorParameters = { "isDefend", "MoveVal", "MoveState", };

    // luma普攻和技能伤害
    private float lunaDamage = -1f;

    private float MonsterDamage = -1f;

    private float lunaMoveDuration = 0.5f;
    private float lunaAttackAnimatorDuration = 0.667f;
    private float lunaFadeDuration = 0.333f;
    private float lunaFadeSize = 0.3f;

    //private float lunaSkillDuration = 0.5f;
    private float lunaSkillEffectDuration = 1.18f; //放在怪物身上的技能伤害动画持续时间

    private float monsterMoveDuration = 0.5f;
    private float monsterFadeDuration = 0.6f;
    private float monsterFade = 0.3f;

    // 动画序列在状态机里的名字
    private string clipNameAtk = "Attack";

    private string clipNameHurt = "Hurt";
    private string clipNameDefense = "isDefend";
    private string clipNameSkill = "Skill";

    // 协程引用
    private IEnumerator lunaAttack, monsterAttack;

    private void Awake() {
        monsterInitPos = monsterTransform.localPosition;
        lunaInitPos = lunaTransform.localPosition;

        lunaAttack = PerformAttackLogic();
        monsterAttack = MonsterAttack();
    }

    /// <summary>
    /// luna的攻击功能
    /// </summary>
    public void Attack() {
        StartCoroutine(PerformAttackLogic());
    }

    /// <summary>
    /// luna的防御功能
    /// </summary>
    public void Defense() {
        StartCoroutine(PerformDefenseLogic());
    }

    public void Skill() {
        if (!GameManager.Instance.CanUseSkill(GameManager.Instance.lunaMpCost)) {
            return;
        }
        StartCoroutine(PerformSkillLogic());
    }

    /// <summary>
    /// 重置精灵渲染器的颜色和alpha值,颜色默认为白色
    /// </summary>
    /// <param name="obj">某个物体的精灵渲染器</param>
    public void SpriteRendererReset(SpriteRenderer obj) {
        obj.color = Color.white;
        // 设置怪物的透明度为100%
        obj.DOFade(1f, 0f);
    }

    /// <summary>
    /// 怪物扣血或者加血,渐变恢复
    /// </summary>
    /// <param name="value"></param>
    public void JudgeMonsterHp(float value = 1f) {
        GameManager.Instance.InOrDecreaseMonsterHp(value);
    }

    /// <summary>
    /// luna扣血或者加血
    /// </summary>
    /// <param name="value"></param>
    public void JudgeLunaHp(float value) {
        GameManager.Instance.InOrDecreaseLunaHp(value);
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
                monsterRenderer.DOFade(monsterFade, monsterFadeDuration)
                    .OnComplete(() => {
                        SpriteRendererReset(monsterRenderer);
                        JudgeMonsterHp(lunaDamage);
                    });
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
    /// <param name="isDefend">luna是否在防御状态</param>
    /// <returns></returns>
    private IEnumerator MonsterAttack() {
        // 怪物先移动到luna旁边然后等待时间再进行冲刺
        monsterTransform.DOLocalMoveX(lunaTransform.localPosition.x - 1.5f, monsterMoveDuration);
        yield return new WaitForSeconds(0.5f);
        // 怪物冲剂
        monsterTransform.DOLocalMoveX(lunaTransform.localPosition.x - 0.5f, monsterMoveDuration / 2f).OnComplete(() => {
            lunaAnimator.CrossFade(clipNameHurt, 0);
            lunaRenderer.color = Color.red;
            lunaRenderer.DOFade(lunaFadeSize, lunaFadeDuration);
        });

        yield return new WaitForSeconds(monsterMoveDuration / 2f + lunaFadeDuration);
        SpriteRendererReset(lunaRenderer);
        JudgeLunaHp(MonsterDamage);

        monsterTransform.DOLocalMove(monsterInitPos, monsterMoveDuration).OnComplete(() => {
            UIManager.Instance.ShowBattleUI(true);
        });
    }

    /// <summary>
    /// 协程,执行Luna防御功能
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformDefenseLogic() {
        UIManager.Instance.ShowBattleUI(false);
        lunaAnimator.SetBool(clipNameDefense, true);

        monsterTransform.DOLocalMoveX(lunaTransform.localPosition.x - 1.5f, monsterMoveDuration / 2f);
        yield return new WaitForSeconds(0.5f);

        // 怪物冲刺
        monsterTransform.DOLocalMoveX(lunaTransform.localPosition.x - 0.5f, monsterMoveDuration / 2f);

        // luna防御后退
        lunaTransform.DOLocalMoveX(lunaTransform.localPosition.x + 1.5f, lunaMoveDuration / 2f).OnComplete(() => {
            // luna防御后退完成然后归位
            lunaTransform.DOLocalMoveX(lunaInitPos.x, lunaMoveDuration / 2f);
        });

        yield return new WaitForSeconds(lunaMoveDuration);
        //怪物归位
        monsterTransform.DOLocalMove(monsterInitPos, monsterMoveDuration).OnComplete(() => {
            UIManager.Instance.ShowBattleUI(true);
            lunaAnimator.SetBool(clipNameDefense, false);
        });
    }

    private IEnumerator PerformSkillLogic() {
        UIManager.Instance.ShowBattleUI(false);
        lunaAnimator.CrossFade(clipNameSkill, 0);
        GameManager.Instance.InOrDecreaseLunaMp(GameManager.Instance.lunaMpCost);

        // 以monster为父类生成在monster脚下的动画
        skillEffectCopy = Instantiate(SkillEffect, monsterTransform) as GameObject;
        monsterRenderer.DOFade(monsterFade, monsterFadeDuration);
        monsterRenderer.color = Color.blue;

        yield return new WaitForSeconds(lunaSkillEffectDuration);

        JudgeMonsterHp(lunaDamage);
        SpriteRendererReset(monsterRenderer);
        UIManager.Instance.ShowBattleUI(true);
    }
}