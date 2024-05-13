﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsManager : MonoBehaviour {

    public enum MissionsName {
        Welcome, PetTheDog, FindCandles, KillMonsters, FinishAll
    }

    private int Candle;
    private int Monsters;
    private string WeaponName = "蓝纹火锤";
    public static MissionsManager Instance;
    public List<Mission> Missions;
    public int DialogIndex; // 当前List中的相应任务的普通对话数组的索引
    public int MissionsIndex; // 任务对话数组的一维索引下标控制,也就是当前正在进行的任务索引

    private void Start() {
        MissionsIndex = 1;
        DialogIndex = 0;
        Instance = this;
        this.Candle = GameManager.Instance.TargetCandleNum;
        this.Monsters = GameManager.Instance.TargetKilledNum;
        Missions = new List<Mission>() {
            // 索引0,任务1 欢迎
            new Mission(MissionsName.Welcome,new DialogInfo[]
            {
                new(){Name = GameManager.NpcNames.Luna,Content = "(,,·V·)\"hello，我是Luna，你可以用上下左右控制我移动，空格键与NPC进行对话，战斗中需要简单点击按钮执行相应行为"},
            },null),

            // 索引1,任务2, 摸狗
            new Mission(MissionsName.PetTheDog,new DialogInfo[]
            {
                new (){Name=GameManager.NpcNames.Luna,Content="好久不见了,小猫咪（*ΦωΦ*）,Luna~"},
                new (){Name=GameManager.NpcNames.Nala,Content="好久不见，Nala，你还是那么有活力，哈哈"},
                new (){Name=GameManager.NpcNames.Luna,Content="还好吧~"},
                new (){Name=GameManager.NpcNames.Nala,Content="我的狗一直在叫，但是我这会忙不过来，你能帮我安抚一下它吗"},
                new (){Name=GameManager.NpcNames.Luna,Content="啊？"},
                new (){Name=GameManager.NpcNames.Nala,Content="摸摸他就行，摸摸说呦西呦西，真是个好孩子呐"},
                new (){Name=GameManager.NpcNames.Luna,Content="别看他叫的这么凶，其实他就是想引起别人的注意"},
                new (){Name=GameManager.NpcNames.Nala,Content="可是。。。。"},
                new (){Name=GameManager.NpcNames.Luna,Content="我是猫女郎啊"},
                new (){Name=GameManager.NpcNames.Nala,Content="安心啦，不会咬你的,去吧去吧~"},
            },new DialogInfo(){Name = GameManager.NpcNames.Luna,Content = "他还在叫呢"}),

            // 索引2,任务3, 找蜡烛
            new Mission(MissionsName.FindCandles,new DialogInfo[]
            {
                new (){Name=GameManager.NpcNames.Nala,Content="感谢你呐，Luna，你还是那么可靠!"},
                new (){Name=GameManager.NpcNames.Nala,Content="我想请你帮个忙好吗"},
                new (){Name=GameManager.NpcNames.Nala,Content="说起来这事怪我。。。"},
                new (){Name=GameManager.NpcNames.Nala,Content="今天我睡过头了，出门比较匆忙"},
                new (){Name=GameManager.NpcNames.Nala,Content="然后装蜡烛的袋子口子没封好！"},
                new (){Name=GameManager.NpcNames.Nala,Content="结果就。。。蜡烛基本丢完了"},
                new (){Name=GameManager.NpcNames.Luna,Content="你还是老样子，哈哈"},
                new (){Name=GameManager.NpcNames.Nala,Content="所以，所以喽，你帮帮忙，帮我把蜡烛找回来"},
                new (){Name=GameManager.NpcNames.Nala,Content="如果你能帮我找回全部的"+Candle+"根蜡烛，我就送你一把神器"},
                new (){Name=GameManager.NpcNames.Luna,Content="神器？"},
                new (){Name=GameManager.NpcNames.Nala,Content="是的，我感觉很适合你，加油呐~"},
            },new DialogInfo(){Name = GameManager.NpcNames.Nala,Content = "你还没帮我收集到所有的蜡烛，宝~"}),

            // 索引3,任务4, 获得武器,可以触发显示怪物,然后清理怪物
            new Mission(MissionsName.KillMonsters,new DialogInfo[]
            {
                new (){Name=GameManager.NpcNames.Nala,Content="可靠啊！竟然一个不差的全收集回来"},
                new (){Name=GameManager.NpcNames.Luna,Content="你知道多累吗？"},
                new (){Name=GameManager.NpcNames.Luna,Content="你到处跑，真的很难收集"},
                new (){Name=GameManager.NpcNames.Nala,Content="辛苦啦辛苦啦"},
                new (){Name=GameManager.NpcNames.Nala,Content="这是给你的奖励"},
                new (){Name=GameManager.NpcNames.Nala,Content= WeaponName + "，传说中的神器"},
                new (){Name=GameManager.NpcNames.Nala,Content="应该挺适合你的"},
                new (){Name=GameManager.NpcNames.Luna,Content="~~获得"+WeaponName+"~~(遇到怪物可触发战斗)"},
                new (){Name=GameManager.NpcNames.Luna,Content="哇，谢谢你！Thanks（·ω·)"},
                new (){Name=GameManager.NpcNames.Nala,Content="嘿嘿（*^v^*），咱们的关系不用客气"},
                new (){Name=GameManager.NpcNames.Nala,Content="正好，最近山里出现了一堆怪物，你也算为民除害，请帮忙清理("+ Monsters +")只怪物"},
                new (){Name=GameManager.NpcNames.Luna,Content="啊？"},
                new (){Name=GameManager.NpcNames.Luna,Content="这才是你的真实目的吧？！"},
                new (){Name=GameManager.NpcNames.Nala,Content="托拜托啦，否则真的很不方便我卖东西"},
                new (){Name=GameManager.NpcNames.Luna,Content="无语中。。。"},
                new (){Name=GameManager.NpcNames.Nala,Content="求求你了~"},
                new (){Name=GameManager.NpcNames.Luna,Content="哎，行吧，谁让你大呢~"},
                new (){Name=GameManager.NpcNames.Nala,Content="嘻嘻，那辛苦你啦"},
            },new DialogInfo(){Name=GameManager.NpcNames.Nala,Content="宝，你还没清理干净呢，这样我不方便嘛~"}),

            // 任务5 完成所有任务
            new Mission(MissionsName.FinishAll,new DialogInfo[]
            {
                new DialogInfo(){Name = GameManager.NpcNames.Nala,Content = "真棒，"+GameManager.NpcNames.Luna+",周围的居民都会十分感谢你的，有机会来我家喝一杯吧"},
                new DialogInfo(){Name = GameManager.NpcNames.Luna,Content = "我觉得可行，哈哈~"},
            },new DialogInfo(){ Name = GameManager.NpcNames.Nala, Content = "改天再见喽~" }),

            // 任务6 ...
            //new Mission("E",new DialogInfo[]
            //{
            //},new DialogInfo(){}),
        };
    }

    /// <summary>
    /// 根据任务名字查找任务
    /// </summary>
    /// <param name="mName">任务名</param>
    /// <returns>任务</returns>
    public Mission FindMission(MissionsManager.MissionsName mName) {
        foreach (Mission mission in Missions) {
            if (mission.Name == mName) return mission;
        }
        return null;
    }
}