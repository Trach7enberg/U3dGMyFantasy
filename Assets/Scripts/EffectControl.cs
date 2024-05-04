using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 控制血瓶、恢复、星星动画效果结束销毁时间
 */

public class EffectControl : MonoBehaviour {

    public void SetDestroyTime(float time) {
        StartCoroutine(PerformDestroy(time));
    }

    public void SetActive(bool isAct) {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 让效果出现一会然后消失
    /// </summary>
    /// <param name="time">持续时间</param>
    public void SetActive(float time) {
        StartCoroutine(PerformActive(time));
    }

    /// <summary>
    /// 执行让效果出现一会然后消失的协程
    /// </summary>
    /// <param name="time">持续时间</param>
    /// <returns></returns>
    private IEnumerator PerformActive(float time) {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        yield return 0;
    }

    /// <summary>
    /// 执行销毁物体的协程
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator PerformDestroy(float time) {
        if (time >= 0) Destroy(this.gameObject, time);
        yield return 0;
    }
}