using Michsky.MUIP;
using UnityEngine;

public class MainWindowManager : MonoBehaviour {
    public static MainWindowManager Instance;

    [SerializeField] private WindowManager myWindowManager;

    private void Awake() {
        Instance = this;
    }

    private void YourFunction() {
        myWindowManager.OpenWindow("Your Window Name"); // open a specific window
        myWindowManager.OpenWindowByIndex(1); // open a specific window by index
        myWindowManager.NextWindow(); // open next page
        myWindowManager.PrevWindow(); // open previous page
        myWindowManager.ShowCurrentWindow(); // show current window
        myWindowManager.HideCurrentWindow(); // hide current window
        myWindowManager.ShowCurrentButton(); // show current window button
        myWindowManager.HideCurrentButton(); // hide current window button
    }
}