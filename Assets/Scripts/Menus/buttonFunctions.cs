using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] Canvas pauseMenuUI, optionsUI, creditsUI, newOrLoadUI;
    public void Play()
    {
        newOrLoadUI.sortingOrder = 2;
    }

    public void Resume()
    {
        gameManager.instance.activeCanvas.enabled = false;
        gameManager.instance.stateUnpaused();
    }

    public void Respawn()
    {
        gameManager.instance.activeCanvas.enabled = false;
        gameManager.instance.stateUnpaused();
        gameManager.instance.playerScript.spawnPlayer();
    }

    public void Restart()
    {
        gameManager.instance.activeCanvas.enabled = false;
        gameManager.instance.stateUnpaused();
        // Load Level 1
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        // Make sure cursor is visible and time is unpaused if player presses "play" button 
        gameManager.instance.stateUnpaused();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        //Load Main Menu
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Options()
    {
        //if opening from in-game menus (paused, lose, or win menus)
        if (gameManager.instance.activeCanvas != null)
        {
            gameManager.instance.activeCanvas.enabled = false; // hide the current menu
            gameManager.instance.optionsMenuCanvas.enabled = true; // show the options menu
            gameManager.instance.activeCanvas = gameManager.instance.optionsMenuCanvas; // set active menu to options menu
        }
        else // if opening from the main menu 
        {
            // display options by setting sorting order to 2;
            optionsUI.sortingOrder = 2;
        }
    }

    public void Credits()
    {
        // display credits by setting sorting order to 2;
        creditsUI.sortingOrder = 2;
    }

}
