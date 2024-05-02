using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public GameObject battleBackGround;

    // 是否能控制luna
    public bool canControlLuna;

    public float lunaMaxHp { get; private set; }
    public float lunaMaxMp { get; private set; }
    public float monsterMaxHp { get; private set; }

    // luna使用Mp技能消耗的蓝数
    public float lunaSkillMpCost;

    // luna使用回血技能消耗的蓝数
    public float lunaHealMpCost;

    [Range(0, 5)]
    public float lunaCurrentHp;

    [Range(0, 5)]
    public float lunaCurrentMp;

    [Range(0, 5)]
    public float monsterCurrentHp;

    // 对话数组的一维索引下标控制
    public int CurrentDialogInfoIndex;

    // 是否抚摸狗子了
    public bool HasPetTheDog;

    public int CandleNum;
    public int KilledMonsterNum;

    // 方便测试
    public bool Test;

    private void Awake() {
        Instance = this;
        canControlLuna = true;
        HasPetTheDog = false;
        CandleNum = 5;
        KilledMonsterNum = 5;
        Test = false;

        CurrentDialogInfoIndex = 0;

        lunaMaxHp = 5;
        lunaMaxMp = 5;
        monsterMaxHp = 5;
        lunaSkillMpCost = -3f;
        lunaHealMpCost = -1f;

        monsterCurrentHp = monsterMaxHp;
        lunaCurrentHp = lunaMaxHp;
        lunaCurrentMp = lunaMaxMp;
    }

    private void Update() {
        UpdateBar();
    }

    /// <summary>
    /// luna增加血量
    /// </summary>
    /// <param name="hp">血量值</param>
    public void InOrDecreaseLunaHp(float hp = 1) {
        lunaCurrentHp = Mathf.Clamp(lunaCurrentHp + hp, 0, lunaMaxHp);
    }

    public void InOrDecreaseLunaMp(float mp = 1) {
        lunaCurrentMp = Mathf.Clamp(lunaCurrentMp + mp, 0, lunaMaxMp);
    }

    public void InOrDecreaseMonsterHp(float hp = 1) {
        monsterCurrentHp = Mathf.Clamp(monsterCurrentHp + hp, 0, monsterMaxHp);
    }

    /// <summary>
    /// 血条和蓝条UI更新
    /// </summary>
    private void UpdateBar() {
        // 蓝条功能暂时没有完成,待更新
        UIManager.Instance.SetBar(lunaCurrentHp / lunaMaxHp, lunaCurrentMp / lunaMaxMp);

        // 测试
        if (Test) {
            lunaCurrentMp = lunaMaxMp;
            lunaCurrentHp = lunaMaxMp;
            monsterCurrentHp = monsterMaxHp;
        }
    }

    /// <summary>
    /// 判断luna是否能回血
    /// </summary>
    /// <returns></returns>
    public bool CanIncreaseLunaHp() {
        return lunaCurrentHp < lunaMaxHp;
    }

    /// <summary>
    /// 判断luna是否能使用消耗蓝量的技能
    /// </summary>
    /// <param name="value">耗蓝数(为负数)</param>
    /// <returns></returns>
    public bool CanUseSkill(float value) {
        //耗蓝数为负数,判断时候就要改回正数
        return lunaCurrentMp >= -value;
    }

    /// <summary>
    /// 启用游戏战斗场景
    /// </summary>
    /// <param name="enter">true为开启</param>
    public void ShowBattleGround(bool enter = true) {
        battleBackGround.SetActive(enter);
    }
}