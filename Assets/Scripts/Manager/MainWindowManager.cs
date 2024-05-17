using Michsky.MUIP;
using UnityEngine;

public class MainWindowManager : MonoBehaviour {
    public static MainWindowManager Instance;

    [SerializeField] private WindowManager MainPanel;   // 主窗口的窗口管理器
    [SerializeField] private GameObject GameMain;

    [SerializeField] private SliderManager MainGameVolumeSlider;
    [SerializeField] private SliderManager BattleGameVolumeSlider;
    [SerializeField] private GameObject InMainGameOption;   // 游戏里打开主界面的按钮
    [SerializeField] private GameObject ContentWindows;     // 所有内容窗口的父类

    private void Awake() {
        Instance = this;
        InitSlider(MainGameVolumeSlider);
        InitSlider(BattleGameVolumeSlider);
    }

    private void InitSlider(SliderManager sm) {
        sm.mainSlider.minValue = 0;
        sm.mainSlider.maxValue = 1;
        sm.mainSlider.value = AudioManager.Instance.GetVolume(sm.name); // 当前 slider value
    }

    /// <summary>
    /// 登录按钮,TODO 尚未实现账号验证
    /// </summary>
    public void Login() {
        HideObject(false);
        MainPanel.gameObject.SetActive(false);
        InMainGameOption.SetActive(true);

        // 关闭显示主场景怪兽
        GameUiManager.Instance.ShowMainMonsters(false);

        // 已经登录 就把登录面板的输入模块换成Tip和退出游戏按钮
        // 主界面UI输入模块禁用
        ContentWindows.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(false);
        // 显示tip
        ContentWindows.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
        // 退出游戏按钮 TODO 退出游戏按钮待优化位置
        ContentWindows.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
    }

    public void SetMainGameMusicVolume() {
        AudioManager.Instance.SetMainVolume(MainGameVolumeSlider.mainSlider.value);
    }

    public void SetBattleGameVolume() {
        AudioManager.Instance.SetBattleVolume(BattleGameVolumeSlider.mainSlider.value);
    }

    /// <summary>
    /// 点击取消按钮就退出游戏
    /// </summary>
    public void Cancel() {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    /// <summary>
    /// 打开主界面的窗口
    /// </summary>
    public void ShowPanelPart() {
        //LoginPanelButton.gameObject.SetActive(false);
        // 面板已经打开了
        if (MainPanel.gameObject.activeSelf) {
            // 再点击就关闭
            MainPanel.gameObject.SetActive(false);
            HideObject(false);

            // 否则面板没打开就打开
        } else {
            HideObject(true);
            MainPanel.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 打开主界面UI时隐藏一些游戏物体,以免误触
    /// </summary>
    /// <param name="isFreeze"></param>
    public void HideObject(bool isFreeze = true) {
        GameMain.transform.GetChild(1).GetChild(5).gameObject.SetActive(!isFreeze); // 蜡烛
        GameMain.transform.GetChild(1).GetChild(6).gameObject.SetActive(!isFreeze); // 药瓶
        GameMain.transform.GetChild(2).gameObject.SetActive(!isFreeze);// Luna
        GameUiManager.Instance.ShowLunaPanel(!isFreeze);
    }
}