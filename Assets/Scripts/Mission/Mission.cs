using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 任务类
/// </summary>
public class Mission 
{
    public string Name;
    public bool IsEnd; // 实现再次点击关闭对话功能的标记
    public bool IsClaimed; // 已领取任务
    public bool IsDone; // 任务完成
    public bool IsFirstTimeInEndDialog; // 防止第一次领取任务对话时进入到任务完成的对话,即第一次播放完任务简介再点击就退出
    public DialogInfo[] DialogInfos;    // 普通对话
    public DialogInfo DoneDialogInfo; // 完成任务播放的对话

    public Mission(string name, bool isEnd, bool isDone, DialogInfo[] dialogInfos)
    {
        Name = name;
        IsEnd = isEnd;
        IsDone = isDone;
        IsFirstTimeInEndDialog = true;
        IsClaimed = false;
        DialogInfos = dialogInfos;
    }

    public Mission(string name, DialogInfo[] dialogInfos, bool isEnd = false, bool isDone = false, bool isClaimed = false,bool isFirst =true, DialogInfo doneDialogInfo=null) {
        Name = name;
        IsEnd = isEnd;
        IsDone = isDone;
        IsClaimed = isClaimed;
        DialogInfos = dialogInfos;
        DoneDialogInfo = doneDialogInfo;
        IsFirstTimeInEndDialog = isFirst;
    }

    public Mission(string name, DialogInfo[] dialogInfos, DialogInfo doneDialogInfo = null, bool isFirst = true, bool isClaimed = false) {
        Name = name;
        IsEnd = false;
        IsDone = false;
        IsClaimed = isClaimed;
        IsFirstTimeInEndDialog = isFirst;
        DialogInfos = dialogInfos;
        DoneDialogInfo = doneDialogInfo;
    }
}
