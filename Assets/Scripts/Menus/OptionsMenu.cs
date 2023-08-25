using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle fullscreen;
    [SerializeField] GameObject pauseMenu, optionsMenu;
    [SerializeField] Canvas optionsUI;
    void Start()
    {
        if(Screen.fullScreen)
        {
            fullscreen.isOn = true;
        }
        else
        fullscreen.isOn = false; 
    }

    public void ToggleFullScreen()
    {
        if(!fullscreen.isOn)
        {
            Screen.fullScreen = false;
        }
        else
        {
            Screen.fullScreen = true;
        }
    }

   public void Back()
   {
        Scene sceneCurr = SceneManager.GetActiveScene();
        // if options is opened from in game
        if (sceneCurr.name != "Main Menu" && gameManager.instance.activeCanvas == gameManager.instance.optionsMenuCanvas)
        {
            gameManager.instance.optionsMenuCanvas.enabled = false; // hide the options menu
            gameManager.instance.pauseMenuCanvas.enabled = true; // show the pause menu
            gameManager.instance.activeCanvas = gameManager.instance.pauseMenuCanvas; // set active menu to pause menu;
        }
        else if (sceneCurr.name == "Main Menu")// if options is opened from main menu
        {
            optionsUI.sortingOrder = 0;
        }
   }
}
