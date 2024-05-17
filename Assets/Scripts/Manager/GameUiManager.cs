using System;
using System.Collections;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading;
using DG.Tweening;
using Michsky.MUIP;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

/// <summary>
/// UI管理
/// </summary>
public partial class GameUiManager : MonoBehaviour {
    public static GameUiManager Instance;
    public Image hpMaskImage;
    public Image mpMaskImage;
    public Image CurrentCharacterImage;
    public Sprite[] characterSprites;
    public Text NameText;
    public Text ContentText;

    public GameObject BattleChoicePanel;
    public GameObject BattleBackGroundPanel;
    public GameObject TalkPanel;
    [SerializeField] private GameObject LunaPanel;

    public GameObject MainSceneMonsters;

    [SerializeField] private float MonsterHpSliderSpeed = 0.2f; // 怪兽血条跟随移动的速度,值越大越慢,最好不要大于0.5
    [SerializeField] private float PosOffset = 50f;         // 血条位置的偏移值
    [SerializeField] private GameObject BattleSceneMonster; // 战斗场景中的怪兽
    [SerializeField] private SliderManager MonsterSlider;   // 怪兽血条,
    public Vector3 InitBattleMonsterGlobPos;  // 战斗场景中的怪兽初始位置
    public Vector3 InitMonsterSliderGlobPos;  // 怪兽血条的初始位置 (注意,它默认就是屏幕坐标,无需转换)

    /// <summary>
    /// 游戏层级mask
    /// </summary>
    public enum GameLayerMask {
        Npc,
    }

    /// <summary>
    /// 血条和蓝条原始宽度
    /// </summary>
    private float originalSize;

    // Awake先于 start()方法执行
    private void Awake() {
        Instance = this;
        DOTween.SetTweensCapacity(500, 50); // 设置DOTween 可以同时存在多少动画队列
        originalSize = hpMaskImage.rectTransform.rect.width;

        InitMonsterBarPos(PosOffset);
        MonsterSlider.mainSlider.minValue = GameManager.Instance.MonsterMinHp;
        MonsterSlider.mainSlider.maxValue = GameManager.Instance.MonsterMaxHp;
        MonsterSlider.mainSlider.value = GameManager.Instance.MonsterCurrentHp;
    }

    private void Update() {
        UpdateLunaBar(GameManager.Instance.LunaCurrentHp / GameManager.Instance.LunaMaxHp, GameManager.Instance.LunaCurrentMp / GameManager.Instance.LunaMaxMp);
        UpdateMonsterBar();
    }

    /// <summary>
    /// 每次进入战斗场景都需要记录一下怪兽的初始位置(因为实际上它是跟着摄像头移动的,每次的初始位置都不一样)
    /// </summary>
    /// <param name="offset">血条偏移</param>
    private void InitMonsterBarPos(float offset = 0) {
        InitBattleMonsterGlobPos = Camera.main.WorldToScreenPoint(BattleSceneMonster.transform.position);
        InitMonsterSliderGlobPos = InitBattleMonsterGlobPos;
        InitMonsterSliderGlobPos.x += offset;
    }

    /// <summary>
    /// 更新战斗场景怪兽血条的位置,只有怪兽移动的时候才会移动
    /// </summary>
    private void UpdateMonsterBar() {
        MonsterSlider.mainSlider.value = GameManager.Instance.MonsterCurrentHp;
        float distance = Camera.main.WorldToScreenPoint(BattleSceneMonster.transform.position).x - InitBattleMonsterGlobPos.x;
        if (distance != 0) {
            MonsterSlider.transform.DOMoveX(InitMonsterSliderGlobPos.x + distance, MonsterHpSliderSpeed);
        }
    }

    /// <summary>
    /// 同时设置血条和蓝条的宽度图片百分比,1.0为满
    /// </summary>
    /// <param name="hpFillPercent">血条填充百分比</param>
    /// <param name="MpFillPercent">蓝条填充百分比</param>
    public void UpdateLunaBar(float hpFillPercent, float MpFillPercent) {
        // 通过RectTransform的锚点来设置图片的宽度,第一个参数我们用锚点的水平轴向,第二个size是填充的百分比
        hpMaskImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpFillPercent * originalSize);
        mpMaskImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MpFillPercent * originalSize);
    }

    /// <summary>
    /// 是否显示luna的头像和蓝血条
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowLunaPanel(bool isShow = true) {
        LunaPanel.gameObject.SetActive(isShow);
    }

    /// <summary>
    /// 是否显示战斗场景中怪兽的血条
    /// </summary>
    /// <param name="isShow"></param>
    private void ShowMonsterHpSlider(bool isShow = true) {
        MonsterSlider.gameObject.SetActive(isShow);
    }

    public void ShowBattleUi(bool enter = true) {
        BattleChoicePanel.SetActive(enter);
    }

    /// <summary>
    /// 进入或者退出游戏战斗场景时
    /// </summary>
    /// <param name="enter">true为开启</param>
    public void ShowBattleGround(bool enter = true) {
        // 关闭战斗场景时
        if (!enter) {
            GameManager.Instance.CanControlLuna = true;
            StartCoroutine(PerformHideMonsterLogic());

            // luna死亡时,回到主场景血量置为1 TODO 或者luna死后应该返回上个存档点
            if (GameManager.Instance.LunaCurrentHp == 0) {
                GameManager.Instance.LunaCurrentHp = 1;
            }

            // 播放正常音乐
            AudioManager.Instance.PlayMusic(AudioManager.Instance.NormalClip);

            // 开启战斗场景时
        } else {
            // TODO 怪物没有死的时候,重新进入战斗场景的血量应该保持
            GameManager.Instance.MonsterCurrentHp = GameManager.Instance.MonsterMaxHp;

            GameManager.Instance.CanControlLuna = false;
            // 播放战斗音乐
            AudioManager.Instance.PlayMusic(AudioManager.Instance.BattleClip);
        }
        BattleBackGroundPanel.SetActive(enter);
        InitMonsterBarPos(PosOffset);
        ShowMonsterHpSlider(enter);
    }

    /// <summary>
    /// 进入游戏战斗场景,并且隐藏与Luna发生战斗的怪物
    /// </summary>
    /// <param name="monster">与Luna发生战斗的怪物</param>
    public void ShowBattleGround(GameObject monster) {
        monster.SetActive(false);
        ShowBattleGround(true);
    }

    /// <summary>
    /// 关闭任务对话框
    /// </summary>
    /// <param name="isShow">t为开启,f为关闭</param>
    private void ShowTalkPanel(bool isShow = true) {
        TalkPanel.SetActive(isShow);
    }

    /// <summary>
    /// 显示对话内容(包含人物切换、文本内容修改),dialogInfo.content为null则关闭对话框
    /// </summary>
    /// <param name="dialogInfo">对话信息</param>
    public void ShowNpcDialog(DialogInfo dialogInfo) {
        if (dialogInfo is not { Content: not null }) {
            ShowTalkPanel(false);
        } else {
            ShowTalkPanel(true);
            CurrentCharacterImage.sprite = characterSprites[(int)dialogInfo.Name];
            CurrentCharacterImage.SetNativeSize();// 即图片组件里的Set Native Size
            NameText.text = dialogInfo.Name.ToString();
            ContentText.text = dialogInfo.Content;
        }
    }

    /// <summary>
    /// 主场景中的N个怪物是否全部启用
    /// </summary>
    /// <param name="isShow">t为显示,f为关闭</param>
    public void ShowMainMonsters(bool isShow) {
        MainSceneMonsters.SetActive(isShow);
    }

    /// <summary>
    /// 返回主场景处理与Luna战斗的怪物是否现身
    /// 退出战斗场景时候怪物如果没有被杀死,则在主场景中现身,需要等待n秒后怪物才能恢复,不然会和Luna撞到一起
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformHideMonsterLogic() {
        if (GameManager.Instance.MonsterCurrentHp != 0) {
            yield return new WaitForSeconds(GameManager.Instance.ShowMonsterTime);
            GameManager.Instance.GetCurrentMonster().SetActive(true);
        }
        yield return 0;
    }

    /// <summary>
    /// 战斗场景是否打开
    /// </summary>
    /// <returns>t为是,f为否</returns>
    public Boolean InBattleGround() {
        return BattleBackGroundPanel.activeSelf;
    }
}