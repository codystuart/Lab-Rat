using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{

    [SerializeField] GameObject optionsMenu;
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
        //Load Main Menu
        SceneManager.LoadScene(0);
    }

    //inventory buttons in progress
    public void use()
    {
    
    }
    
    public void drop()
    {
    
    }

    public void quit()
    {
        Application.Quit();
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
    }

    public void Credits()
    {
        Debug.Log("Credits loaded");
    }
}
