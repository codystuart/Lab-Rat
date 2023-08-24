using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player Objects -----")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;

    [Header("----- UI Menus -----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public GameObject respawnMenu;
    public GameObject optionsMenu;

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

    //class references
    //public int enemiesRemaining;
    public bool isPaused;
    public float timescaleOrig;
    private float secondsCount;
    private int minuteCount;
    private bool pauseTimer;
    public bool keycardAcquired;
    
    //Cure collection and counting variables
    public int totalCureCount;
    int cureCollected;

    public bool collectedAllCures;
    private GameObject[] findCures;

    // [Header("----- Puzzle -----")]
    // [SerializeField] List<GameObject> correctBtnOrder = new List<GameObject>();
    // [SerializeField] List<GameObject> btnPressedOrder = new List<GameObject>();
    // [SerializeField] GameObject keycard;
    // [SerializeField] GameObject tryAgainPuzzleText;
    // [SerializeField] GameObject keycardAcquiredText;
    // public bool correctOrder = false;

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
            save.saveGunList.Clear();
        }

        if (sceneCurr.name == "Level 1" || sceneCurr.name == "TestingWaveSpawner")
        {
            clearSave();
        }

        playerScript.gunList = save.saveGunList;
        playerScript.selectedGun = 0; //set selected gun to first in list


        ////sets lists of saved items
        //inventorySystem.inventory.items = save.saveInvItems;
        //inventorySystem.inventory.notes = save.saveNotes;

        ////sets flashlight meshes on next scene
        //if (save.saveFlashlight)
        //    playerScript.pickupFlashlight();

        batteryChargeBar.fillAmount = 0;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            //if no menu, display pause menu
            activeMenu = pauseMenu;
            activeMenu.SetActive(true);
            statePaused();
        }
        else if (Input.GetButtonDown("Cancel") && activeMenu != null && activeMenu != loseMenu && activeMenu != winMenu && activeMenu != respawnMenu && activeMenu != dialog && activeMenu != optionsMenu)
        {
            //closes menu with escape
            stateUnpaused();
            activeMenu = null;
        }
        else if(Input.GetButtonDown("Cancel") && activeMenu == optionsMenu)
        {
            //close the options menu
            activeMenu.SetActive(false);
            // display pause menu
            activeMenu = pauseMenu;
            activeMenu.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Tab) && activeMenu == null)
        {
            //if tab and no menu is open, open inventory
            openInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && activeMenu == inventory)
        {
            inventorySystem.inventory.selectedItem = null;
            stateUnpaused();
            activeMenu = null;
        }
        if (!pauseTimer)
        {
            updateTimerUI();
        }
    }

    public void clearSave()
    {
        //save.saveGunList.Clear();
        //save.saveInvItems.Clear();
        //save.saveNotes.Clear();
        save.saveFlashlight = false;
    }

    public void statePaused()
    {
        //pauses game
        pauseTimer = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        reticle.SetActive(false);
        //AudioListener.volume = 0; //this stops all sound/ sfx from being heard while paused
        isPaused = !isPaused;
    }

    public void stateUnpaused()
    {
        //unpauses game
        pauseTimer = false;
        Time.timeScale = timescaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        reticle.SetActive(true);
        //AudioListener.volume = 1; //this turns the volume back on
        isPaused = !isPaused;
        activeMenu.SetActive(false);

        //clean up
        if (displayName.text != string.Empty)
            displayName.text = string.Empty;
        if (itemDescription.text != string.Empty)
            itemDescription.text = string.Empty;
        if (noteDescription.text != string.Empty)
            noteDescription.text = string.Empty;

        activeMenu = null;
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

    public void doDialog()
    {
        //starts preset dialog
        activeMenu = dialog;
        activeMenu.SetActive(true);
        pauseTimer = true;
    }

    public void youWin()
    {
        //opens win menu
        activeMenu = winMenu;
        activeMenu.SetActive(true);
        statePaused();
    }
    public void youLose()
    {
        //opens lose menu
        statePaused();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    public void RespawnLevel()
    {
        //opens respawn menu
        statePaused();
        activeMenu = respawnMenu;
        activeMenu.SetActive(true);
    }

    // void updateCounters()
    // {
    //     //updated game goal labels
    //     enemiesRemainingText.text = enemiesRemaining.ToString("F0");
    //     cureBottlesRemainingText.text = cureCollected.ToString("F0") + "/" + totalCureCount.ToString("F0");
    // }

    void updateTimerUI()
    {
        //timer logic
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
    // public void ButtonPressedOrder(GameObject button)
    // {
    //     //button puzzle logic
    //     Debug.Log("Button Added To list");
    //     btnPressedOrder.Add(button);

    //     if(btnPressedOrder.Count == correctBtnOrder.Count)
    //     {
    //         for (int i = 0; i < correctBtnOrder.Count; i++)
    //         {
    //             if (correctBtnOrder[i] == btnPressedOrder[i])
    //             {
    //                 correctOrder = true;
    //             }
    //             else
    //             {
    //                 correctOrder = false;
    //                 break;
    //             }
    //         }
    //         if (correctOrder)
    //         {
    //             StartCoroutine(KeycardAcquired());
    //             Instantiate(keycard, player.transform.position, player.transform.rotation);
    //         }
    //         else
    //         {
    //             StartCoroutine(FailedPuzzle());
    //             btnPressedOrder.Clear();
    //             // play error sound
    //         }
    //     }
    // }
    // IEnumerator KeycardAcquired()
    // {
    //     keycardAcquiredText.SetActive(true);
    //     yield return new WaitForSeconds(1f);
    //     keycardAcquiredText.SetActive(false);
    // }
    // IEnumerator FailedPuzzle()
    // {
    //     tryAgainPuzzleText.SetActive(true);
    //     yield return new WaitForSeconds(2f);
    //     tryAgainPuzzleText.SetActive(false);
    // }
}