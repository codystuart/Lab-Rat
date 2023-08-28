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
    public Canvas activeCanvas, previousCanvas;
    public Canvas pauseMenuCanvas, optionsMenuCanvas, respawnMenuCanvas, winMenuCanvas, gameOverMenuCanvas;

    [Header("----- UI Text -----")]
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
    public AudioSource inGameMusic;
    public GameObject inGameSFX;
    public bool isPaused;
    public float timescaleOrig;
    public bool keycardAcquired;
    public bool cureCollected; //win condition

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
            //sets flashlight meshes after level 1
            if (save.saveFlashlight)
                playerScript.pickupFlashlight();
        }
        else
        {
            //only clears on level 1
            clearPlayer();
            clearSave();
        }
        
        //sets player data to saved data
        playerScript.gunList = save.saveGunList;
        playerScript.selectedGun = 0;
        inventorySystem.inventory.items = save.saveInvItems;
        inventorySystem.inventory.notes = save.saveNotes;
        playerScript.fLight.intensity = save.fLightIntensity;
        playerScript.updatePlayerUI();
    }

    private void Start() {
        pauseMenuCanvas = pauseMenuCanvas.GetComponent<Canvas>();
        optionsMenuCanvas = optionsMenuCanvas.GetComponent<Canvas>();
        respawnMenuCanvas = respawnMenuCanvas.GetComponent<Canvas>();
    }

    void Update()
    {
        // Opens pause menu
        if((Input.GetButtonDown("Cancel") || (Input.GetKeyDown(KeyCode.P))) && activeMenu == null && activeCanvas == null) 
        {
            pauseMenuCanvas.enabled = true;
            activeCanvas = pauseMenuCanvas;
            previousCanvas = activeCanvas;
            statePaused();
        }
        // Closes pause menu
        else if ((Input.GetButtonDown("Cancel") || (Input.GetKeyDown(KeyCode.P))) && activeCanvas == pauseMenuCanvas)
        {
            source.PlayOneShot(menuBackSfx);
            pauseMenuCanvas.enabled = false;
            activeCanvas = null;
            previousCanvas = null;
            stateUnpaused();
        }
        // Closes options menu, returns to last menu
        else if ((Input.GetButtonDown("Cancel") || (Input.GetKeyDown(KeyCode.P))) && activeCanvas == optionsMenuCanvas)
        {
            source.PlayOneShot(menuBackSfx);
            optionsMenuCanvas.enabled = false;
            previousCanvas.enabled = true;
            activeCanvas = previousCanvas;
        }
        else if ((Input.GetButtonDown("Cancel") || (Input.GetKeyDown(KeyCode.P))) && activeMenu != null && activeMenu != dialog)
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
    }

    public void clearPlayer()
    {
        //clears player data
        cureCollected = false;
        playerScript.hasFlashlight = false;
        playerScript.fLight.intensity = 0;
        playerScript.gunList.Clear();
        inventorySystem.inventory.items.Clear();
        inventorySystem.inventory.notes.Clear();
    }

    public void clearSave()
    {
        //clears save data
        save.saveFlashlight = false;
        save.fLightIntensity = 0;
        save.saveGunList.Clear();
        save.saveInvItems.Clear();
        save.saveNotes.Clear();
    }

    public void statePaused()
    {
        //pauses game
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
        previousCanvas = null;
    }
    public IEnumerator playerFlashDamage()
    {
        //player damage UI
        playerFlashDamagePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerFlashDamagePanel.SetActive(false);
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
    }

    public void youWin()
    {
        //opens win menu
        winMenuCanvas.enabled = true;
        activeCanvas = winMenuCanvas;
        previousCanvas = activeCanvas;
        statePaused();
    }
    public void youLose()
    {
        //opens lose menu
        gameOverMenuCanvas.enabled = true;
        activeCanvas = gameOverMenuCanvas;
        previousCanvas = activeCanvas;
        statePaused();
    }

    public void RespawnLevel()
    {
        //opens respawn menu
        respawnMenuCanvas.enabled = true; 
        activeCanvas = respawnMenuCanvas;
        previousCanvas = activeCanvas;
        statePaused();
    }
}