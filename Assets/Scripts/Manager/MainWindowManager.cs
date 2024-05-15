using Michsky.MUIP;
using UnityEngine;

public class MainWindowManager : MonoBehaviour {
    public static MainWindowManager Instance;

    [SerializeField] private WindowManager MainPanel;
    [SerializeField] private GameObject GameMain;

    private void Awake() {
        Instance = this;
    }

    private void YourFunction() {
        MainPanel.OpenWindow("Your Window Name"); // open a specific window
        MainPanel.OpenWindowByIndex(1); // open a specific window by index
        MainPanel.NextWindow(); // open next page
        MainPanel.PrevWindow(); // open previous page
        MainPanel.ShowCurrentWindow(); // show current window
        MainPanel.HideCurrentWindow(); // hide current window
        MainPanel.ShowCurrentButton(); // show current window button
        MainPanel.HideCurrentButton(); // hide current window button
    }

    public void Login() {
        GameMain.transform.GetChild(1).GetChild(5).gameObject.SetActive(true);
        GameMain.transform.GetChild(1).GetChild(6).gameObject.SetActive(true);
        GameMain.transform.GetChild(2).gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}