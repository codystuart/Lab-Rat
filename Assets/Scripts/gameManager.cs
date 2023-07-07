using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player Stuff -----")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;

    [Header("----- UI Stuff -----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI cureBottlesRemainingText;
    public GameObject playerFlashDamagePanel;
    public GameObject reticle;
    public Image playerHpBar;

    int enemiesRemaining;
    bool isPaused;
    float timescaleOrig;
    public int totalCureCount;
    int cureCollected;
    bool collectedAllCures; 

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
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
    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        updateCounters();

        if (enemiesRemaining <= 0 && collectedAllCures)
        {
            activeMenu = winMenu;
            activeMenu.SetActive(true);
            statePaused();
        }

    }

    public void updateCureGameGoal(int amount) 
    { 
        cureCollected += amount;
        updateCounters();

        if (cureCollected == totalCureCount)
        {
            collectedAllCures = true;
        }
        if (collectedAllCures && enemiesRemaining <= 0)
        {
            activeMenu = winMenu;
            activeMenu.SetActive(true);
            statePaused();
        }

    }
    public void youLose()
    {
        statePaused();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    void updateCounters()
    {
        enemiesRemainingText.text = enemiesRemaining.ToString("F0");
        cureBottlesRemainingText.text = cureCollected.ToString("F0") + "/" + totalCureCount.ToString("F0");
    }
}