using System;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;

public class GameManager : MonoBehaviour {

    /// <summary>
    /// 游戏里的Npc的标签名字
    /// </summary>
    public enum NpcNames {
        Luna, Nala, Dog
    }

    /// <summary>
    /// 动画控制机里的动画触发参数名字
    /// </summary>
    public enum AnimatorParameters {
        ToX, ToY, Jump, Climb, Run, MoveValue, MainMonsterToX, MainMonsterToY
    }

    /// <summary>
    /// 动画控制机里的动画名字
    /// </summary>
    public enum AnimatorMotionName {
        TouchTheDog, LookTheDog, TalkLaugh, DogBeHappy, DogBark,
    }

    public static GameManager Instance;

    public GameObject BattleBackGround;

    // 是否能控制luna
    public bool CanControlLuna;

    public float LunaMaxHp { get; private set; }
    public float LunaMaxMp { get; private set; }
    public float MonsterMaxHp { get; private set; }

    // luna使用Mp技能消耗的蓝数
    public float LunaSkillMpCost;

    // luna使用回血技能消耗的蓝数
    public float LunaHealMpCost;

    [Range(0, 5)] public float LunaCurrentHp;

    [Range(0, 5)] public float LunaCurrentMp;

    [Range(0, 5)] public float MonsterCurrentHp;

    // 对话数组的一维索引下标控制
    public int MissionsIndex;

    // 是否抚摸狗子了
    public bool HasPetTheDog;

    public int CandleNum;
    public int KilledMonsterNum;

    // 方便测试
    public bool Test;

    /// <summary>
    /// 其它类需要用到此类的方法和属性,所以是Awake不能是Start
    /// </summary>
    private void Awake() {
        Instance = this;
        CanControlLuna = true;
        HasPetTheDog = false;
        CandleNum = 5;
        KilledMonsterNum = 5;
        Test = false;

        MissionsIndex = 1;

        LunaMaxHp = 5;
        LunaMaxMp = 5;
        MonsterMaxHp = 5;
        LunaSkillMpCost = -3f;
        LunaHealMpCost = -1f;

        MonsterCurrentHp = MonsterMaxHp;
        LunaCurrentHp = LunaMaxHp;
        LunaCurrentMp = LunaMaxMp;
    }

    private void Update() {
        UpdateBar();
    }

    /// <summary>
    /// luna增加血量
    /// </summary>
    /// <param name="hp">血量值</param>
    public void InOrDecreaseLunaHp(float hp = 1) {
        LunaCurrentHp = Mathf.Clamp(LunaCurrentHp + hp, 0, LunaMaxHp);
    }

    public void InOrDecreaseLunaMp(float mp = 1) {
        LunaCurrentMp = Mathf.Clamp(LunaCurrentMp + mp, 0, LunaMaxMp);
    }

    public void InOrDecreaseMonsterHp(float hp = 1) {
        MonsterCurrentHp = Mathf.Clamp(MonsterCurrentHp + hp, 0, MonsterMaxHp);
    }

    /// <summary>
    /// 血条和蓝条UI更新
    /// </summary>
    private void UpdateBar() {
        UiManager.Instance.SetBar(LunaCurrentHp / LunaMaxHp, LunaCurrentMp / LunaMaxMp);

        // 测试
        if (Test) {
            LunaCurrentMp = LunaMaxMp;
            LunaCurrentHp = LunaMaxMp;
            MonsterCurrentHp = MonsterMaxHp;
        }
    }

    /// <summary>
    /// 判断luna是否能回血
    /// </summary>
    /// <returns></returns>
    public bool CanIncreaseLunaHp() {
        return LunaCurrentHp < LunaMaxHp;
    }

    /// <summary>
    /// 判断luna是否能使用消耗蓝量的技能
    /// </summary>
    /// <param name="value">耗蓝数(为负数)</param>
    /// <returns></returns>
    public bool CanUseSkill(float value) {
        //耗蓝数为负数,判断时候就要改回正数
        return LunaCurrentMp >= -value;
    }

    /// <summary>
    /// 启用游戏战斗场景
    /// </summary>
    /// <param name="enter">true为开启</param>
    public void ShowBattleGround(bool enter = true) {
        BattleBackGround.SetActive(enter);
    }
}