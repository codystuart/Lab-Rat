using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player Objects -----")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;

    [Header("----- UI Objects -----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI cureBottlesRemainingText;
    public TextMeshProUGUI gameTimer;
    public GameObject playerFlashDamagePanel;
    public GameObject reticle;
    public Image playerHpBar;
    public Image sprintMeter;


    [Header("----- Map Objects ------")]
    [SerializeField] GameObject secretWall;

    int enemiesRemaining;
    bool isPaused;
    float timescaleOrig;
    private float secondsCount;
    private int minuteCount;
    private bool pauseTimer;

    //Cure collection and counting variables
    public int totalCureCount;
    int cureCollected;

    bool collectedAllCures;
    private GameObject[] findCures;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        timescaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");

        //Alternate method to count total cures?
        findCures = GameObject.FindGameObjectsWithTag("Cure");
        totalCureCount = findCures.Length;
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            statePaused();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }
        if (!pauseTimer)
        {
            updateTimerUI();
        }
        
    }

    public void statePaused()
    {
        pauseTimer = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        reticle.SetActive(false);
        isPaused = !isPaused;

    }

    public void stateUnpaused()
    {
        pauseTimer = false;
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

        if (enemiesRemaining <= 0 && secretWall != null)
        {
            secretWall.GetComponent<Renderer>().enabled = false;
            secretWall.GetComponent<Collider>().enabled = false;
        }

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

    void updateTimerUI()
    {
        secondsCount += Time.deltaTime;


        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
        else if (minuteCount >= 60 && secondsCount >= 60)
        {
            youLose();
        }

        int secondsToInt = (int)secondsCount;

        gameTimer.text = "Time " + minuteCount.ToString("00") + ":" + secondsToInt.ToString("00");
    }
}