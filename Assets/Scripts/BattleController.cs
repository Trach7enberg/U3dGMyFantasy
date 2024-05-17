using System.Collections;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 战斗场景的战斗控制
/// </summary>
public class BattleController : MonoBehaviour {
    public Animator lunaAnimator;
    public Transform lunaTransform;
    public Transform monsterTransform;
    public GameObject SkillEffect;
    public GameObject HealEffect;
    private GameObject skillEffectCopy;
    private GameObject healEffectCopy;

    private Vector3 monsterInitPos;
    private Vector3 lunaInitPos;

    public SpriteRenderer monsterRenderer;
    public SpriteRenderer lunaRenderer;

    private string[] animatorParameters = { "isDefend", "MoveVal", "MoveState", };

    // luma普攻和技能伤害
    private float lunaDamage = -1f;

    private float lunaSkillDamage = -3f;

    private float MonsterDamage = -1f;

    private float lunaMoveDuration = 0.5f;
    private float lunaAttackAnimatorDuration = 0.667f;
    private float lunaFadeDuration = 0.333f;
    private float lunaFadeSize = 0.3f;

    //private float lunaRecoverHpDuration = 1f;
    private float lunaDieDuration = 1f;

    //private float lunaHurtDuration = 0.417f; // luna受击动画时间

    //private float lunaSkillDuration = 0.5f;
    private float lunaSkillEffectDuration = 1.18f; //放在怪物身上的技能伤害动画持续时间

    private float lunaHealEffectDuration = 1.2f;

    private float monsterMoveDuration = 0.5f;
    private float monsterFadeDuration = 0.6f;
    private float monsterFade = 0.3f;
    //private float monsterDieDuration = 1f;

    // 动画序列在状态机里的名字
    private string clipNameAtk = "Attack";

    private string clipNameHurt = "Hurt";
    private string clipNameDefense = "isDefend";
    private string clipNameSkill = "Skill";
    private string clipNameRecoverHp = "RecoverHP";
    private string clipNameDie = "Die";

    // 协程引用
    private IEnumerator lunaAttack, monsterAttack;

    private void Awake() {
        monsterInitPos = monsterTransform.localPosition;
        lunaInitPos = lunaTransform.localPosition;
        lunaAttack = PerformAttackLogic();
        monsterAttack = PerformMonsterAttackLogic();
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

    /// <summary>
    /// luna的伤害技能
    /// </summary>
    public void Skill() {
        if (!GameManager.Instance.CanUseSkill(GameManager.Instance.LunaSkillMpCost)) {
            return;
        }
        StartCoroutine(PerformSkillLogic());
    }

    /// <summary>
    /// luna的回血技能
    /// </summary>
    public void RecoverHp() {
        if (!GameManager.Instance.CanUseSkill(GameManager.Instance.LunaHealMpCost) || !GameManager.Instance.CanIncreaseLunaHp()) {
            return;
        }
        StartCoroutine(PerformRecoverHpLogic());
    }

    public void Escape() {
        GameUiManager.Instance.ShowBattleUi(false);
        lunaAnimator.SetBool(animatorParameters[2], true);
        lunaAnimator.SetFloat(animatorParameters[1], 1f);
        lunaTransform.DOLocalMoveX(lunaTransform.localPosition.x + 2.5f, lunaMoveDuration).OnComplete((() => {
            lunaAnimator.SetBool(animatorParameters[2], false);
            lunaAnimator.SetFloat(animatorParameters[1], 0f);
            GameUiManager.Instance.ShowBattleGround(false);
            lunaTransform.localPosition = lunaInitPos;
        }));
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
    /// 怪物扣血或者加血
    /// </summary>
    /// <param name="value"></param>
    public void JudgeMonsterHp(float value = 1f) {
        GameManager.Instance.InOrDecreaseMonsterHp(value);
        if (GameManager.Instance.MonsterCurrentHp <= 0) {
            PerformMonsterDieLogic();
        }
    }

    /// <summary>
    /// 裁决luna的生命值判断加减和死亡
    /// </summary>
    /// <param name="value">默认值为1,加一滴血</param>
    public void JudgeLunaHp(float value = 1f) {
        GameManager.Instance.InOrDecreaseLunaHp(value);
        if (GameManager.Instance.LunaCurrentHp <= 0) {
            StartCoroutine(PerformLunaDieLogic());
        }
    }

    /// <summary>
    /// 裁决luna的蓝值判断加减
    /// </summary>
    /// <param name="value">默认值为1,回一滴蓝,为负数则扣篮</param>
    public void JudgeLunaMp(float value = 1f) {
        GameManager.Instance.InOrDecreaseLunaMp(value);
    }

    /// <summary>
    /// 协程,执行luna攻击
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformAttackLogic() {
        GameUiManager.Instance.ShowBattleUi(false);
        lunaAnimator.SetBool(animatorParameters[2], true);
        lunaAnimator.SetFloat(animatorParameters[1], -1f);

        // luna前移动攻击
        lunaTransform.DOLocalMoveX(monsterTransform.localPosition.x + 1.5f, lunaMoveDuration)
            .OnComplete(() => {
                lunaAnimator.SetBool(animatorParameters[2], false);
                lunaAnimator.SetFloat(animatorParameters[1], 0f);

                // 直接播放attack动画,结束之后自动游回箭头所指的动作,0代表状态机的层级
                lunaAnimator.CrossFade(clipNameAtk, 0);
                // 播放luna攻击音效
                AudioManager.Instance.PlaySound(AudioManager.Instance.AttackClip);
                AudioManager.Instance.PlaySound(AudioManager.Instance.LunaActionClip);
                JudgeMonsterHp(lunaDamage); // 扣怪物血
                //怪物受击渐变动画
                monsterRenderer.color = Color.red;
                monsterRenderer.DOFade(monsterFade, monsterFadeDuration)
                    .OnComplete(() => {
                        SpriteRendererReset(monsterRenderer);
                    });
            });

        // 每次普通攻击,luna都会回蓝 TODO 逻辑待更新,不应该普攻回蓝、暂时没想到好方法
        JudgeLunaMp();

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
        StartCoroutine(PerformMonsterAttackLogic());
    }

    /// <summary>
    /// 协程,怪物冲刺攻击
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformMonsterAttackLogic() {
        // 预先播放怪兽攻击音效
        AudioManager.Instance.PlaySound(AudioManager.Instance.MonsterAttackClip);
        // 怪物先移动到luna旁边然后等待时间再进行冲刺
        monsterTransform.DOLocalMoveX(lunaTransform.localPosition.x - 1.5f, monsterMoveDuration);
        yield return new WaitForSeconds(0.5f);
        // 怪物冲剂
        monsterTransform.DOLocalMoveX(lunaTransform.localPosition.x - 0.5f, monsterMoveDuration / 2f).OnComplete(() => {
            // 播放luna受击音效
            AudioManager.Instance.PlaySound(AudioManager.Instance.LunaHurtClip, AudioManager.Instance.VolumeScale * 2f);
            lunaAnimator.CrossFade(clipNameHurt, 0);
            lunaRenderer.color = Color.red;
            lunaRenderer.DOFade(lunaFadeSize, lunaFadeDuration);
        });

        yield return new WaitForSeconds(monsterMoveDuration / 2f + lunaFadeDuration);
        SpriteRendererReset(lunaRenderer);
        JudgeLunaHp(MonsterDamage);

        monsterTransform.DOLocalMove(monsterInitPos, monsterMoveDuration).OnComplete(() => {
            GameUiManager.Instance.ShowBattleUi(true);
        });
    }

    /// <summary>
    /// 协程,执行Luna防御功能
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformDefenseLogic() {
        GameUiManager.Instance.ShowBattleUi(false);
        AudioManager.Instance.PlaySound(AudioManager.Instance.LunaActionClip);
        lunaAnimator.SetBool(clipNameDefense, true);

        // 预先播放怪兽攻击音效
        AudioManager.Instance.PlaySound(AudioManager.Instance.MonsterAttackClip);
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
            GameUiManager.Instance.ShowBattleUi(true);
            lunaAnimator.SetBool(clipNameDefense, false);
        });
    }

    /// <summary>
    /// 协程,执行luna的伤害技能
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformSkillLogic() {
        GameUiManager.Instance.ShowBattleUi(false);

        lunaAnimator.CrossFade(clipNameSkill, 0);
        JudgeLunaMp(GameManager.Instance.LunaSkillMpCost);

        AudioManager.Instance.PlaySound(AudioManager.Instance.LunaActionClip);
        AudioManager.Instance.PlaySound(AudioManager.Instance.FinishActionClip, AudioManager.Instance.VolumeScale);
        // 以monster为父类生成在monster脚下的动画
        skillEffectCopy = Instantiate(SkillEffect, monsterTransform) as GameObject;
        skillEffectCopy.GetComponent<EffectControl>().SetDestroyTime(1f);

        monsterRenderer.DOFade(monsterFade, monsterFadeDuration);
        monsterRenderer.color = Color.red;

        yield return new WaitForSeconds(lunaSkillEffectDuration);

        //monsterRenderer.color = Color.white;
        //monsterRenderer.DOFade(1f, 0)
        //    .OnComplete(() => {  });
        SpriteRendererReset(monsterRenderer);
        JudgeMonsterHp(lunaSkillDamage);

        //注意:开启协程 会导致怪物死亡用技能时再次战斗会有bug,所以需要判断是否退出战场
        if (GameUiManager.Instance.BattleBackGroundPanel.activeSelf != false) {
            StartCoroutine(PerformMonsterAttackLogic());
        }
    }

    /// <summary>
    /// 协程,执行luna的回血技能
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformRecoverHpLogic() {
        GameUiManager.Instance.ShowBattleUi(false);
        AudioManager.Instance.PlaySound(AudioManager.Instance.LunaActionClip);
        AudioManager.Instance.PlaySound(AudioManager.Instance.RecoverHpClip, AudioManager.Instance.VolumeScale * 2f);
        lunaAnimator.CrossFade(clipNameRecoverHp, 0);
        JudgeLunaMp(GameManager.Instance.LunaHealMpCost);

        healEffectCopy = Instantiate(HealEffect, lunaTransform) as GameObject;
        healEffectCopy.GetComponent<EffectControl>().SetDestroyTime(1f);

        yield return new WaitForSeconds(lunaHealEffectDuration);
        JudgeLunaHp();
        GameUiManager.Instance.ShowBattleUi(true);
        yield return null;
    }

    /// <summary>
    /// luna 死亡
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformLunaDieLogic() {
        // 播放luna死亡音效
        AudioManager.Instance.PlaySound(AudioManager.Instance.FinishActionClip, AudioManager.Instance.VolumeScale * 2f);
        lunaAnimator.CrossFade(clipNameDie, 0);
        lunaRenderer.color = Color.red;
        lunaRenderer.DOFade(lunaFadeDuration, lunaDieDuration);
        yield return new WaitForSeconds(lunaDieDuration);
        GameUiManager.Instance.ShowBattleUi(false);
        GameUiManager.Instance.ShowBattleGround(false);
        SpriteRendererReset(lunaRenderer);
    }

    /// <summary>
    /// 执行怪物死亡
    /// </summary>
    public void PerformMonsterDieLogic() {
        // 播放怪物死亡音效
        AudioManager.Instance.PlaySound(AudioManager.Instance.MonsterDieClip, AudioManager.Instance.VolumeScale * 2f);
        // 怪物击杀加一
        GameManager.Instance.KilledNum++;
        GameUiManager.Instance.ShowBattleUi(false);
        GameUiManager.Instance.ShowBattleGround(false);
        SpriteRendererReset(monsterRenderer);
    }
}