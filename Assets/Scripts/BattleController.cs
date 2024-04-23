using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ս��������ս������
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

    // luma�չ��˺�
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
    /// �����Ѫ(��δ���),����ָ�
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
                
                // ֱ�Ӳ���attack����,����֮���Զ��λؼ�ͷ��ָ�Ķ���,0����״̬���Ĳ㼶
                lunaAnimator.CrossFade("LunaAttack", 0);
                //�����ܻ����䶯��
                monsterRenderer.color = Color.red;
                monsterRenderer.DOFade(0.4f, monsterFadeDuration).OnComplete(() => { JudgeMonsterHP(lunaDamage); });

            });
        yield return null;
    }
}
