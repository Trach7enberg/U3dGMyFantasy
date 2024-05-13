using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 任务对话信息控制
/// </summary>
public class NpcDialog : MonoBehaviour {
    public Animator NalaAnimator;
    public Animator DogAnimator;
    public GameObject StarEffect;

    /// <summary>
    /// 显示对话
    /// </summary>
    public void DisplayDialog() {
        if (MissionsManager.Instance.MissionsIndex < MissionsManager.Instance.Missions.Count) {
            Mission m = MissionsManager.Instance.Missions[MissionsManager.Instance.MissionsIndex];

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
    }
}