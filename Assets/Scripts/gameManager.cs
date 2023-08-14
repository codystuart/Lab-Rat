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

    [Header("----- UI Text -----")]
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI cureBottlesRemainingText;
    public TextMeshProUGUI gameTimer;
    public TextMeshProUGUI currAmmoText;
    public TextMeshProUGUI maxAmmoText;

    [Header("----- UI Objects -----")]
    public GameObject playerFlashDamagePanel;
    public GameObject reticle;
    public GameObject noAmmo;
    public GameObject noGun;
    public Image playerHpBar;
    public Image sprintMeter;
    public Image batteryChargeBar;

    [Header("----- Inventory Objects -----")]
    public GameObject inventory;
    public GameObject batteryMeter;
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

    //class references
    public int enemiesRemaining;
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

    [Header("----- Puzzle -----")]
    [SerializeField] List<GameObject> correctBtnOrder = new List<GameObject>();
    [SerializeField] List<GameObject> btnPressedOrder = new List<GameObject>();
    [SerializeField] GameObject keycard;
    [SerializeField] GameObject tryAgainPuzzleText;
    [SerializeField] GameObject keycardAcquiredText;
    public bool correctOrder = false;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        timescaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");

        //Alternate method to count total cures?
        // findCures = GameObject.FindGameObjectsWithTag("Cure");
        // totalCureCount = findCures.Length;
        
        //Sets the list of saved guns and grabbed from the exitDoor script and applies it to the character when loading into a new level, and sets it to the first gun in the list
        playerScript.gunList = save.gunListSave;
        playerScript.selectedGun = 0;

        UnityEngine.SceneManagement.Scene sceneCurr = SceneManager.GetActiveScene();
        string sceneName = sceneCurr.name;
        if (sceneName == "Level 1" || sceneName == "TestingWaveSpawner")
        {
            save.gunListSave.Clear();
        }
        
        batteryMeter.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            //if no menu, display pause menu
            statePaused();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }
        else if (Input.GetButtonDown("Cancel") && activeMenu != null && activeMenu != loseMenu && activeMenu != winMenu)
        {
            //closes menu with escape
            stateUnpaused();
            activeMenu = null;
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

    public void statePaused()
    {
        //pauses game
        pauseTimer = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        reticle.SetActive(false);
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
        isPaused = !isPaused;
        activeMenu.SetActive(false);

        //clean up
        if (displayName.text != string.Empty)
            displayName.text = string.Empty;
        if (itemDescription.text != string.Empty)
            itemDescription.text = string.Empty;
        if (noteDescription.text != string.Empty)
            noteDescription.text = string.Empty;

        playerScript.canMove = true;
        activeMenu = null;
    }
    public IEnumerator playerFlashDamage()
    {
        //player damage UI
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
    }
    
    public void updateCureGameGoal(int amount) 
    { 
        cureCollected += amount;
        updateCounters();

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
        if (playerScript.hasFlashlight)
            batteryMeter.SetActive(true);
        statePaused();
        inventorySystem.inventory.ListItems();
    }

    public void doDialog()
    {
        //starts preset dialog
        activeMenu = dialog;
        activeMenu.SetActive(true);
        pauseTimer = true;
        playerScript.canMove = false;
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

    void updateCounters()
    {
        //updated game goal labels
        enemiesRemainingText.text = enemiesRemaining.ToString("F0");
        cureBottlesRemainingText.text = cureCollected.ToString("F0") + "/" + totalCureCount.ToString("F0");
    }

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
    public void ButtonPressedOrder(GameObject button)
    {
        //button puzzle logic
        Debug.Log("Button Added To list");
        btnPressedOrder.Add(button);

        if(btnPressedOrder.Count == correctBtnOrder.Count)
        {
            for (int i = 0; i < correctBtnOrder.Count; i++)
            {
                if (correctBtnOrder[i] == btnPressedOrder[i])
                {
                    correctOrder = true;
                }
                else
                {
                    correctOrder = false;
                    break;
                }
            }
            if (correctOrder)
            {
                StartCoroutine(KeycardAcquired());
                Instantiate(keycard, player.transform.position, player.transform.rotation);
            }
            else
            {
                StartCoroutine(FailedPuzzle());
                btnPressedOrder.Clear();
                // play error sound
            }
        }
    }
    IEnumerator KeycardAcquired()
    {
        keycardAcquiredText.SetActive(true);
        yield return new WaitForSeconds(1f);
        keycardAcquiredText.SetActive(false);
    }
    IEnumerator FailedPuzzle()
    {
        tryAgainPuzzleText.SetActive(true);
        yield return new WaitForSeconds(2f);
        tryAgainPuzzleText.SetActive(false);
    }
}