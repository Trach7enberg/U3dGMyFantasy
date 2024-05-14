using Michsky.MUIP;
using UnityEngine;

public class MainUiManager : MonoBehaviour
{
    public static MainUiManager Instance;
    
    [SerializeField] private WindowManager myWindowManager;

   


    void Awake()
    {
       Instance = this;
        
    }

  
    void YourFunction() {
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
