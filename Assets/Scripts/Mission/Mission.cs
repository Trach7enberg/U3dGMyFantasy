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
    public bool IsEnd; // ʵ���ٴε���رնԻ����ܵı��
    public bool IsClaimed; // ����ȡ����
    public bool IsDone; // �������
    public bool IsFirstTimeInEndDialog; // ��ֹ��һ����ȡ����Ի�ʱ���뵽������ɵĶԻ�,����һ�β������������ٵ�����˳�
    public DialogInfo[] DialogInfos;    // ��ͨ�Ի�
    public DialogInfo DoneDialogInfo; // ������񲥷ŵĶԻ�

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
