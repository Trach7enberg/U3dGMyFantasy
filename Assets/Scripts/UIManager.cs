using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 对话信息结构体
/// </summary>
public struct DialogInfo {
    public UIManager.NpcNames Name;
    public string Content;
}

/// <summary>
/// UI管理
/// </summary>
public class UIManager : MonoBehaviour {
    public static UIManager Instance;
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

    public enum NpcNames {
        Luna, Nala
    }

    /// <summary>
    /// 游戏层级mask
    /// </summary>
    public enum GameLayerMask {
        NPC,
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

    public void ShowTalkPanel(bool isShow = true) {
        TalkPanel.SetActive(isShow);
    }

    /// <summary>
    /// 显示对话内容(包含人物切换、文本内容修改),content为null则关闭对话框
    /// </summary>
    /// <param name="eName">枚举类型的npc名字</param>
    /// <param name="content">对话的信息</param>
    public void ShowNpcDialog(NpcNames eName = NpcNames.Luna, string content = null) {
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

    /// <summary>
    /// 主场景怪物是否启用
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowMonsters(bool isShow) {
        MainSceneMonsters.SetActive(isShow);
    }
}