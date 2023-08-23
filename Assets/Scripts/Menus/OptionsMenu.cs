using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        // hide options by setting sorting order to 0;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
            gameManager.instance.activeMenu = pauseMenu;
            optionsMenu.SetActive(false);
        }
        else
        {
            optionsUI.sortingOrder = 0;
        }
   }
}
