using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle fullscreen;
    [SerializeField] GameObject optionsMenu;
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
// Apply changes
//     public void ApplyChanges()
//    {
//         if(!fullscreen.isOn)
//         {
//             Screen.fullScreen = false;
//         }
//         else
//         {
//             Screen.fullScreen = true;
//         }
//    } 

   public void Back()
   {
    optionsMenu.SetActive(false);
   }
}
