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
    /// 关闭任务对话框
    /// </summary>
    /// <param name="isShow">t为开启,f为关闭</param>
    private void ShowTalkPanel(bool isShow = true) {
        TalkPanel.SetActive(isShow);
    }

    /// <summary>
    /// 显示对话内容(包含人物切换、文本内容修改),content为null则关闭对话框
    /// </summary>
    /// <param name="eName">枚举类型的npc名字</param>
    /// <param name="content">对话的信息</param>
    public void ShowNpcDialog(GameManager.NpcNames eName = GameManager.NpcNames.Luna, string content = null) {
        if (content == null) {
            ShowTalkPanel(false);
        } else {
            ShowTalkPanel(true);
            CurrentCharacterImage.sprite = characterSprites[(int)eName];
            CurrentCharacterImage.SetNativeSize();// 即图片组件里的Set Native Size
            NameText.text = eName.ToString();
            ContentText.text = content;
        }
    }

    public void ShowNpcDialog(DialogInfo dialogInfo) {
        if (dialogInfo == null || dialogInfo.Content == null) {
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
    /// 主场景怪物是否启用
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowMonsters(bool isShow) {
        MainSceneMonsters.SetActive(isShow);
    }
}