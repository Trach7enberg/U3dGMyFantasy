using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 任务对话信息控制
/// </summary>
public class NpcDialog : MonoBehaviour {
    public Animator NalaAnimator;
    public Animator DogAnimator;

    /// <summary>
    /// 显示对话
    /// </summary>
    public void DisplayDialog() {
        if (GameManager.Instance.MissionsIndex < MissionsManager.Instance.Missions.Count) {
            Mission m = MissionsManager.Instance.Missions[GameManager.Instance.MissionsIndex];

            if (!m.IsDone) {
                DialogInfo info;
                if (m.IsEnd) {
                    info = null;
                    m.IsEnd = false;
                    GameManager.Instance.CanControlLuna = true;
                } else if (MissionsManager.Instance.DialogIndex < m.DialogInfos.Length) {
                    info = m.DialogInfos[MissionsManager.Instance.DialogIndex++];
                    GameManager.Instance.CanControlLuna = false;
                } else {
                    if (m.IsFirstTimeInEndDialog) {
                        info = null;
                        m.IsFirstTimeInEndDialog = false;
                        GameManager.Instance.CanControlLuna = true;
                        m.IsClaimed = true;
                        m.IsEnd = false;
                    } else {
                        info = m.DoneDialogInfo;
                        GameManager.Instance.CanControlLuna = false;
                        m.IsEnd = true;
                    }
                }
                UiManager.Instance.ShowNpcDialog(info);
            }
        }
        //Debug.Log("listCurrentLen: "+ GameManager.Instance.MissionsIndex+ ",listLen: "+ (Dialoglist.Count - 1));
        // 当前一维索引大于主数组下标则返回
        //if (!(GameManager.Instance.MissionsIndex < Dialoglist.Count)) {
        //    return;
        //}

        // 当前list里的某个内容数组播放完成时,检测任务完成状况
        //if (CurrentIndex >= Dialoglist[GameManager.Instance.MissionsIndex].Length) {
        //    switch (GameManager.Instance.MissionsIndex) {
        //        case 2 when
        //            !GameManager.Instance.HasPetTheDog:
        //            // 还没有完成摸狗子任务时
        //            break;

        //        case 4 when
        //            GameManager.Instance.CandleNum < TargetCandles:
        //            break;

        //        case 6 when
        //            GameManager.Instance.KilledMonsterNum < TargetKill:
        //            break;

        //        default:
        //            GameManager.Instance.MissionsIndex++;
        //            CurrentIndex = 0;
        //            break;
        //    }

        //    // 主场景怪物显示
        //    if (GameManager.Instance.MissionsIndex == 6) {
        //        UIManager.Instance.ShowMonsters(true);
        //    }

        //    UIManager.Instance.ShowNpcDialog();
        //    GameManager.Instance.canControlLuna = true;
        //} else {
        //    GameManager.Instance.canControlLuna = false;
        //    DialogInfo info = Dialoglist[GameManager.Instance.MissionsIndex][CurrentIndex++];
        //    UIManager.Instance.ShowNpcDialog(info.Name, info.Content);
        //}
    }
}