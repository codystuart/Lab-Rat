using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player Stuff -----")]
    public GameObject player;
    public GameObject meleeAttackTrigger;
    public playerController playerScript;
    public GameObject playerSpawnPos;

    [Header("----- UI Stuff -----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject playerFlashDamagePanel;
    public GameObject reticle;
    public Image playerHpBar;

    bool isPaused;
    float timescaleOrig;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        meleeAttackTrigger = GameObject.FindGameObjectWithTag("Can Hit Player");
        playerScript = player.GetComponent<playerController>();
        timescaleOrig = Time.timeScale;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            statePaused();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }
    }

    public void statePaused()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        reticle.SetActive(false);
        isPaused = !isPaused;
    }

    public void stateUnpaused()
    {
        Time.timeScale = timescaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        reticle.SetActive(true);
        isPaused = !isPaused;
        activeMenu.SetActive(false);
        activeMenu = null;
    }
    public IEnumerator playerFlashDamage()
    {
        playerFlashDamagePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerFlashDamagePanel.SetActive(false);
    }

    public void youLose()
    {
        statePaused();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
}