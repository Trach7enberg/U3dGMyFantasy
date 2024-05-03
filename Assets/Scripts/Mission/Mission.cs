using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ������
/// </summary>
public class Mission 
{
    public string Name;
    public bool IsEnd;
    public bool IsDone;
    public DialogInfo[] DialogInfos;    // ��ͨ�Ի�
    public DialogInfo DoneDialogInfo; // ������񲥷ŵĶԻ�

    public Mission(string name, bool isEnd, bool isDone, DialogInfo[] dialogInfos)
    {
        Name = name;
        IsEnd = isEnd;
        IsDone = isDone;
        DialogInfos = dialogInfos;
    }

    public Mission(string name, DialogInfo[] dialogInfos, bool isEnd = false, bool isDone = false, DialogInfo doneDialogInfo=null) {
        Name = name;
        IsEnd = isEnd;
        IsDone = isDone;
        DialogInfos = dialogInfos;
        DoneDialogInfo = doneDialogInfo;
    }

    public Mission(string name, DialogInfo[] dialogInfos, DialogInfo doneDialogInfo = null) {
        Name = name;
        IsEnd = false;
        IsDone = false;
        DialogInfos = dialogInfos;
        DoneDialogInfo = doneDialogInfo;
    }
}
