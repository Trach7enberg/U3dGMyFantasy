using System.Collections;
using System.Security.Cryptography;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

/// <summary>
/// UI管理
/// </summary>
public partial class UiManager : MonoBehaviour {
    public static UiManager Instance;
    public Image hpMaskImage;
    public Image mpMaskImage;
    public Image CurrentCharacterImage;
    public Sprite[] characterSprites;
    public Text NameText;
    public Text ContentText;

    public GameObject BattleChoicePanel;
    public GameObject BattleBackGroundPanel;
    public GameObject TalkPanel;

    public GameObject MainSceneMonsters;
    public GameObject MainGameObject;

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
        originalSize = hpMaskImage.rectTransform.rect.width;
    }

    /// <summary>
    /// 同时设置血条和蓝条的宽度图片百分比,1.0为满
    /// </summary>
    /// <param name="hpFillPercent">血条填充百分比</param>
    /// <param name="MpFillPercent">蓝条填充百分比</param>
    public void SetBar(float hpFillPercent, float MpFillPercent) {
        // 通过RectTransform的锚点来设置图片的宽度,第一个参数我们用锚点的水平轴向,第二个size是填充的百分比
        hpMaskImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpFillPercent * originalSize);
        mpMaskImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MpFillPercent * originalSize);
    }

    public void ShowBattleUi(bool enter = true) {
        BattleChoicePanel.SetActive(enter);
    }

    /// <summary>
    /// 进入或者退出游戏战斗场景
    /// </summary>
    /// <param name="enter">true为开启</param>
    public void ShowBattleGround(bool enter = true) {
        // 关闭战斗场景时
        if (!enter) {
            GameManager.Instance.CanControlLuna = true;
            StartCoroutine(PerformMonsterLogic());
            // 播放正常音乐
            AudioManager.Instance.PlayMusic(AudioManager.Instance.NormalClip);
        } else {
            GameManager.Instance.MonsterCurrentHp = GameManager.Instance.MonsterMaxHp;
            GameManager.Instance.CanControlLuna = false;
            // 播放战斗音乐
            AudioManager.Instance.PlayMusic(AudioManager.Instance.BattleClip);
        }
        BattleBackGroundPanel.SetActive(enter);
    }

    /// <summary>
    /// 进入游戏战斗场景,并且隐藏与Luna发生战斗的怪物
    /// </summary>
    /// <param name="monster">与Luna发生战斗的怪物</param>
    /// <param name="battleLuna">战斗场景中的luna</param>
    /// <param name="m">monster的初始位置</param>
    /// <param name="bL">battleLuna的初始位置</param>
    /// <param name="enter">true为开启</param>
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
    /// <param name="isShow"></param>
    public void ShowMonsters(bool isShow) {
        MainSceneMonsters.SetActive(isShow);
    }

    /// <summary>
    /// 返回主场景处理与Luna战斗的怪物是否现身
    /// 退出战斗场景时候怪物如果没有被杀死,则在主场景中现身,需要等待n秒后怪物才能恢复,不然会和Luna撞到一起
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformMonsterLogic() {
        if (GameManager.Instance.MonsterCurrentHp != 0) {
            yield return new WaitForSeconds(GameManager.Instance.ShowMonsterTime);
            GameManager.Instance.GetCurrentMonster().SetActive(true);
        }
        yield return 0;
    }

    public void ShowMainGameObject(bool isShow = true) {

    }
}