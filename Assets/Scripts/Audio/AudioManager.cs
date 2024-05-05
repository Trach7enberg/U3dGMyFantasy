using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏音乐控制
/// </summary>
public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    public AudioSource MainAudioSource;

    // 正常场景音频
    public AudioClip NormalClip;

    // 战斗场景音频
    public AudioClip BattleClip;

    // 攻击音效
    public AudioClip AttackClip;

    // 狗叫音效
    public AudioClip DogBarkClip;

    // 完成任务、互动物品等音效
    public AudioClip FinishActionClip;

    // 脚步音效
    public AudioClip FootStepLowClip;

    // luna在战斗场景中做动作的音效,是喊声
    public AudioClip LunaActionClip;

    // luna死亡音效
    public AudioClip LunaDieClip;

    // luna受伤音效
    public AudioClip LunaHurtClip;

    // monster攻击音效,Cut版本为缩短版本的音效
    public AudioClip MonsterAttackClip;

    // monster死亡音效,Cut版本为缩短版本的音效 TODO 待缩短
    public AudioClip MonsterDieClip;

    // luna回血音效,Cut版本为缩短版本的音效
    public AudioClip RecoverHpClip;

    // luna用技能攻击音效,Cut版本为缩短版本的音效
    public AudioClip LunaSkillClip;

    public void Awake() {
        Instance = this;
        // 游戏打开就播放音乐
        MainAudioSource.volume = 0.10f;
        MainAudioSource.Play();
    }

    /// <summary>
    /// 播放持续不断的音乐
    /// </summary>
    /// <param name="audioClip"></param>
    public void PlayMusic(AudioClip audioClip) {
        if (MainAudioSource.clip != audioClip) {
            MainAudioSource.clip = audioClip;
            MainAudioSource.Play();
        }
    }

    /// <summary>
    /// 播放一次音频
    /// </summary>
    /// <param name="audioClip">音频</param>
    /// <param name="volumeScale">播放的音频声音大小</param>
    public void PlaySound(AudioClip audioClip, float volumeScale = 1f) {
        if (audioClip) {
            MainAudioSource.PlayOneShot(audioClip, volumeScale);
        }
    }
}