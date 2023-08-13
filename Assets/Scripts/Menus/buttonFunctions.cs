using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] Canvas mainMenuUI, optionsUI, creditsUI;
    public void play()
    {
        // Load Level 1
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void resume()
    {
        gameManager.instance.stateUnpaused();
    }

    public void respawn()
    {
        gameManager.instance.stateUnpaused();
        gameManager.instance.playerScript.spawnPlayer();
    }

    public void restart()
    {
        gameManager.instance.stateUnpaused();
        // Load Level 1
        SceneManager.LoadScene(1);
    }

    public void mainMenu()
    {
        // Make sure cursor is visible and time is unpaused if player presses "play" button 
        gameManager.instance.stateUnpaused();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        //Load Main Menu
        SceneManager.LoadScene(0);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void Options()
    {
        //if opening from pause menu, hide the pause menu
        if (pauseMenu != null && gameManager.instance.activeMenu == gameManager.instance.pauseMenu)
            pauseMenu.SetActive(false);

        // display options by setting sorting order to 2;
        optionsUI.sortingOrder = 2;
    }

    public void Credits()
    {
        // display credits by setting sorting order to 2;
        creditsUI.sortingOrder = 2;
    }
}
