using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Search;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player Objects -----")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;

    [Header("----- UI Menus -----")]
    public GameObject activeMenu;
    public Canvas activeCanvas;
    public Canvas pauseMenuCanvas, optionsMenuCanvas, respawnMenuCanvas, winMenuCanvas, gameOverMenuCanvas;

    [Header("----- UI Text -----")]
    //public TextMeshProUGUI enemiesRemainingText;
    //public TextMeshProUGUI cureBottlesRemainingText;
    public TextMeshProUGUI gameTimer;
    public TextMeshProUGUI currAmmoText;
    public TextMeshProUGUI maxAmmoText;
    public TextMeshProUGUI objectiveText;

    [Header("----- UI Objects -----")]
    public GameObject playerFlashDamagePanel;
    public GameObject reticle;
    public GameObject noAmmo;
    public GameObject noGun;
    public GameObject needBattery;
    public Image playerHpBar;
    public Image sprintMeter;
    public Image batteryChargeBar;

    [Header("----- Inventory Objects -----")]
    public GameObject inventory;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI itemDescription;

    [Header("----- Notes Objects -----")]
    public GameObject notes;
    public TextMeshProUGUI noteDescription;

    [Header("----- Dialog Objects -----")]
    public GameObject dialog;

    [Header("----- Map Objects ------")]
    [SerializeField] GameObject secretWall;
    public saveStats save;

    [Header("----- SFX -----")]
    public AudioSource flashlightON;
    public AudioSource flashlightOFF;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip menuBackSfx;
    [SerializeField] AudioSource inGameMusic;
    [SerializeField] GameObject inGameSFX;

    //class references
    //public int enemiesRemaining;
    public bool isPaused;
    public float timescaleOrig;
    private float secondsCount;
    private int minuteCount;
    //private bool pauseTimer;
    public bool keycardAcquired;
    
    //Cure collection and counting variables
    public int totalCureCount;
    int cureCollected;

    public bool collectedAllCures;
    private GameObject[] findCures;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        timescaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");

        Scene sceneCurr = SceneManager.GetActiveScene();
        if (sceneCurr.name != "Level 1")
        {
            if (save.saveFlashlight)
                playerScript.pickupFlashlight();
        }
        else
        {
            clearSave();
        }

        if (sceneCurr.name == "Level 1" || sceneCurr.name == "TestingWaveSpawner")
        {
            playerScript.hasFlashlight = false;
            playerScript.gunList.Clear();
            inventorySystem.inventory.items.Clear();
            inventorySystem.inventory.notes.Clear();
        }

        playerScript.gunList = save.saveGunList;
        playerScript.selectedGun = 0;
        inventorySystem.inventory.items = save.saveInvItems;
        inventorySystem.inventory.notes = save.saveNotes;
        

        batteryChargeBar.fillAmount = 0;
    }
    private void Start() {
        pauseMenuCanvas = pauseMenuCanvas.GetComponent<Canvas>();
        optionsMenuCanvas = optionsMenuCanvas.GetComponent<Canvas>();
        respawnMenuCanvas = respawnMenuCanvas.GetComponent<Canvas>();
    }
    void Update()
    {

        // Opens pause menu
        if(Input.GetButtonDown("Cancel") && activeMenu == null && activeCanvas == null) 
        {
            pauseMenuCanvas.enabled = true;
            //activeMenu = pauseMenu;
            activeCanvas = pauseMenuCanvas;
            statePaused();
        }
        // Closes pause menu
        else if (Input.GetButtonDown("Cancel") && activeCanvas == pauseMenuCanvas)
        {
            source.PlayOneShot(menuBackSfx);
            pauseMenuCanvas.enabled = false;
            //activeMenu = null;
            activeCanvas = null;
            stateUnpaused();
        }
        // Closes options menu, returns to pause menu
        else if (Input.GetButtonDown("Cancel") && activeCanvas == optionsMenuCanvas)
        {
            source.PlayOneShot(menuBackSfx);
            optionsMenuCanvas.enabled = false;
            pauseMenuCanvas.enabled = true;
            //activeMenu = pauseMenu;
            activeCanvas = pauseMenuCanvas;
        }
        else if (Input.GetButtonDown("Cancel") && activeMenu != null && activeMenu != dialog)
        {
            //closes menu with escape
            stateUnpaused();
            activeMenu = null;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && activeMenu == null && activeCanvas == null)
        {
            //if tab and no menu is open, open inventory
            openInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && (activeMenu == inventory || activeMenu == notes) && activeCanvas == null)
        {
            inventorySystem.inventory.selectedItem = null;
            closeInventory();
        }
        // if (!pauseTimer)
        // {
        //     updateTimerUI();
        // }
    }

    public void clearSave()
    {
        save.saveGunList.Clear();
        save.saveInvItems.Clear();
        save.saveNotes.Clear();
        save.saveFlashlight = false;
    }

    public void statePaused()
    {
        //pauses game
        //pauseTimer = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        reticle.SetActive(false);

        // pause IN GAME sound effects and IN GAME music
        if(activeCanvas != null)
        {
            inGameMusic.Pause();
            inGameSFX.SetActive(false);
        }

        isPaused = !isPaused;
    }

    public void stateUnpaused()
    {
        //unpauses game
        //pauseTimer = false;
        Time.timeScale = timescaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        reticle.SetActive(true);

        // unpause IN GAME sound effects and IN GAME music
        if(activeCanvas == null)
        {
            inGameMusic.UnPause();
            inGameSFX.SetActive(true);
        }

        isPaused = !isPaused;
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }

        //clean up
        if (displayName.text != string.Empty)
            displayName.text = string.Empty;
        if (itemDescription.text != string.Empty)
            itemDescription.text = string.Empty;
        if (noteDescription.text != string.Empty)
            noteDescription.text = string.Empty;

        activeMenu = null;
        activeCanvas = null;
    }
    public IEnumerator playerFlashDamage()
    {
        //player damage UI
        playerFlashDamagePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerFlashDamagePanel.SetActive(false);
    }

    // public void updateGameGoal(int amount)
    // {
    //     enemiesRemaining += amount;
    //     //updateCounters();

    //     if (enemiesRemaining <= 0 && secretWall != null)
    //     {
    //         secretWall.GetComponent<Renderer>().enabled = false;
    //         secretWall.GetComponent<Collider>().enabled = false;
    //     }
    // }
    
    public void updateCureGameGoal(int amount) 
    { 
        cureCollected += amount;
        //updateCounters();

        if (cureCollected == totalCureCount)
        {
            collectedAllCures = true;
        }
    }

    public void openInventory()
    {
        //opens inventory screen
        activeMenu = inventory;
        activeMenu.SetActive(true);
        statePaused();
        inventorySystem.inventory.ListItems();
    }

    public void closeInventory()
    {
        activeMenu.SetActive(false);
        activeMenu = null;
        stateUnpaused();
    }

    public void doDialog()
    {
        //starts preset dialog
        activeMenu = dialog;
        activeMenu.SetActive(true);
        //pauseTimer = true;
    }

    public void youWin()
    {
        //opens win menu
        // activeMenu = winMenu;
        // activeMenu.SetActive(true);
        statePaused();
    }
    public void youLose()
    {
        //opens lose menu
        statePaused();
        // activeMenu = loseMenu;
        //activeMenu.SetActive(true);
    }

    public void RespawnLevel()
    {
        respawnMenuCanvas.enabled = true; //opens respawn menu
        activeCanvas = respawnMenuCanvas;
        statePaused();
        // activeMenu = respawnMenu;
        //activeMenu.SetActive(true);
    }

    // void updateTimerUI()
    // {
    //     //timer logic
    //     secondsCount += Time.deltaTime;


    //     if (secondsCount >= 60)
    //     {
    //         minuteCount++;
    //         secondsCount = 0;
    //     }
    //     else if (minuteCount >= 60 && secondsCount >= 60)
    //     {
    //         youLose();
    //     }

    //     int secondsToInt = (int)secondsCount;

    //     gameTimer.text = "Time " + minuteCount.ToString("00") + ":" + secondsToInt.ToString("00");
    // }

}