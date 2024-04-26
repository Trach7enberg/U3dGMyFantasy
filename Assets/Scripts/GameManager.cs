using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public GameObject battleBackGround;

    public float lunaMaxHp { get; private set; }
    public float lunaMaxMp { get; private set; }
    public float monsterMaxHp { get; private set; }

    public float lunaSkillMpCost;
    public float lunaHealMpCost;

    [Range(0, 5)]
    public float lunaCurrentHp;

    [Range(0, 5)]
    public float lunaCurrentMp;

    [Range(0, 5)]
    public float monsterCurrentHp;

    private void Awake() {
        Instance = this;

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
    public void EnterOrExitBattle(bool enter = true) {
        battleBackGround.SetActive(enter);
    }
}