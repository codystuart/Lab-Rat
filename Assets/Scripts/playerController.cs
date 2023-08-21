using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Player -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] CapsuleCollider charCollider;

    [Header("----- Player Stats -----")]
    [Range(1, 10)]public int HP;
    [SerializeField] public int lives;
    [Range(1, 10)][SerializeField] float playerSpeed;
    [Range(2, 5)] [SerializeField] float sprintDuration;
    [Range(1, 5)][SerializeField] int sprintCooldownLength;

    [Header("----- Jumping -----")]
    [Range(5,10)][SerializeField] float jumpHeight;
    [SerializeField] float gravity;
    [SerializeField] int jumpMax;

    [Header("----- Crouching -----")]
    [SerializeField] Vector3 crouchCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] Vector3 standCenter = new Vector3(0, 0, 0);
    [SerializeField] float crouchHeight = 0.5f;
    [SerializeField] float standingHeight = 2f;
    [SerializeField] float timeToCrouch = 0.25f;
    public bool isCrouching; //enemies will need access for level 2 to despawn

    [Header("----- Gun Stats -----")]
    public List<gunStats> gunList = new List<gunStats>();
    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject muzzleFlash;
    [Range(0f, 2f)][SerializeField] float shootRate;
    [Range(0, 10)][SerializeField] int shootDamage;
    [Range(0, 100)][SerializeField] int shootDist;

    [Header("----- Flashlight -----")]
    [SerializeField] GameObject flashlightModel;
    public GameObject flashlight;
    [SerializeField] Light fLight;
    public bool drainOverTime;
    public float maxBrightness;
    public float minBrightness;
    public float drainRate;

    [Header("----- Class Objects -----")]
    public int originalHP;
    public int originalLives;
    public int selectedGun;
    public bool hasFlashlight;
    public bool fLightIsOn;
    public bool hasPhone;
    Vector3 move;
    Vector3 velocity;
    int jumpCount;
    bool playerGrounded;
    bool isShooting;
    bool sprintCooldown;
    float playerSpeedOrig;
    bool hasSpareAmmo;

    void Start()
    {
        originalHP = HP;
        originalLives = lives;
        playerSpeedOrig = playerSpeed;
        fLight.enabled = false;
        fLightIsOn = false;

        spawnPlayer();

        if (gunList.Count > 0)
        {
            changeGunStats();
        }
        if (gameManager.instance.save.saveFlashlight)
            hasFlashlight = true;
    }

    void Update()
    {
        if (gameManager.instance.activeMenu == null)
        {
            Movement();
            useFlashlight();

            if (gunList.Count > 0)
            {
                scrollGuns();

                if (Input.GetButton("Shoot") && !isShooting)
                {
                    StartCoroutine(shoot());
                }

                if (Input.GetButton("Reload") && !isShooting && gunList[selectedGun].currAmmo != gunList[selectedGun].maxAmmo)
                {
                    fillAmmo();
                }
            }
        }
    }

    //movement function
    void Movement()
    {
        playerGrounded = controller.isGrounded;

        if (playerGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
            jumpCount = 0;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) +
                (transform.forward * Input.GetAxis("Vertical"));

        controller.Move(playerSpeed * Time.deltaTime * move);

        //make player jump
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            velocity.y = jumpHeight;
            ++jumpCount;
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.C) && playerGrounded)
        {
            StartCoroutine(crouchStand());
        }

        //make player sprint
        if (Input.GetButtonDown("Sprint"))
        {
            if (!sprintCooldown)
            {
                StartCoroutine(sprint());
            }
        }
    }

    IEnumerator crouchStand()
    {
        //check to see if anything is above player
        //limited to one unit above player so that ceiling doesn't count
        if (isCrouching && Physics.Raycast(Camera.main.transform.position, Vector3.up, 1f))
            yield break;

        float timeElapsed = 0;

        //change height bassed on crouching bool
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currHeight = controller.height;

        //change center bassed on crouching bool
        Vector3 targetCenter = isCrouching ? standCenter : crouchCenter;
        Vector3 currCenter = controller.center;

        //change height and center
        while (timeElapsed < timeToCrouch)
        {
            controller.height = Mathf.Lerp(currHeight, targetHeight, timeElapsed / timeToCrouch);
            controller.center = Vector3.Lerp(currCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //ensure correct height and center
        controller.height = targetHeight;
        controller.center = targetCenter;

        //set player collider to correct height and center
        charCollider.height = targetHeight;
        charCollider.center = targetCenter;

        //toggle crouching bool
        isCrouching = !isCrouching;
    }

    IEnumerator shoot()
    {
        if (gunList[selectedGun].currAmmo > 0)
        {
            isShooting = true;

            StartCoroutine(flashMuzzle());
            --gunList[selectedGun].currAmmo;
            updatePlayerUI();

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                IDamage damageable = hit.collider.GetComponent<IDamage>();

                if (damageable != null)
                {
                    damageable.TakeDamage(shootDamage);
                }
                else
                    Instantiate(gunList[selectedGun].hitEffect, hit.point, Quaternion.identity);
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
        else
        {
            //if no ammo, display no ammo text
            StartCoroutine(noAmmoLeft());
        }
    }

    IEnumerator flashMuzzle()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }

    IEnumerator noAmmoLeft()
    {
        gameManager.instance.noAmmo.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameManager.instance.noAmmo.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(gameManager.instance.playerFlashDamage());
        updatePlayerUI();

        if (HP <= 0 && lives > 0)
        {
            lives--;
            gameManager.instance.RespawnLevel();
        }
        else if (HP <= 0 && lives <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    public void useFlashlight()
    {
        fLight.intensity = Mathf.Clamp(fLight.intensity, minBrightness, maxBrightness);

        if (drainOverTime && fLight.enabled)
        {
            gameManager.instance.flashlightON.Play();
            if (fLight.intensity > minBrightness)
            {
                fLight.intensity -= Time.deltaTime * (drainRate / 1000);
            }
        }

        if (hasFlashlight && !fLightIsOn)
            gameManager.instance.flashlightOFF.Play();

        if (hasFlashlight && Input.GetKeyDown(KeyCode.F))
        {
            if (fLight.intensity <= minBrightness)
            {
                StartCoroutine(recharge());
            }

            fLight.enabled = !fLight.enabled;
            fLightIsOn = !fLightIsOn;
        }

        if (hasFlashlight)
            gameManager.instance.save.fLightIntensity = fLight.intensity;
        updatePlayerUI();
    }

    IEnumerator recharge()
    {
        gameManager.instance.needBattery.SetActive(true);
        yield return new WaitForSeconds(3f);
        gameManager.instance.needBattery.SetActive(false);
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        if (gameManager.instance.playerSpawnPos != null)
        {
            transform.position = gameManager.instance.playerSpawnPos.transform.position;
        }
        controller.enabled = true;
        HP = originalHP;
        updatePlayerUI();
    }

    IEnumerator sprint()
    {
        playerSpeed = playerSpeed * 1.5f;
        yield return new WaitForSeconds(sprintDuration);
        playerSpeed = playerSpeedOrig;
        StartCoroutine(sprintCooldownTimer());
    }

    IEnumerator sprintCooldownTimer()
    {
        sprintCooldown = true;
        yield return new WaitForSeconds(sprintCooldownLength);
        sprintCooldown = false;
    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHpBar.fillAmount = (float)HP / originalHP;

        if (hasFlashlight)
            gameManager.instance.batteryChargeBar.fillAmount = (float)fLight.intensity / maxBrightness;

        if (gunList.Count > 0)
        {
            gameManager.instance.currAmmoText.text = gunList[selectedGun].currAmmo.ToString("f0");
            gameManager.instance.maxAmmoText.text = gunList[selectedGun].maxAmmo.ToString("f0");
        }
    }

    public void gunPickup(gunStats gunStat)
    {
        //add gun to list, set gun stats, then update UI
        gunList.Add(gunStat);

        shootRate = gunStat.shootRate;
        shootDamage = gunStat.shootDamage;
        shootDist = gunStat.shootDist;

        gunModel.GetComponent<MeshFilter>().mesh = gunStat.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().material = gunStat.gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = gunList.Count - 1;
        updatePlayerUI();
    }

    public void pickupFlashlight()
    {
        hasFlashlight = true;
        flashlightModel.GetComponent<MeshFilter>().mesh = gameManager.instance.save.saveFlashlightPrefab.GetComponent<MeshFilter>().sharedMesh;
        flashlightModel.GetComponent<MeshRenderer>().material = gameManager.instance.save.saveFlashlightPrefab.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void scrollGuns()
    {
        //change gun depending on gun list
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            ++selectedGun;
            changeGunStats();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            --selectedGun;
            changeGunStats();
        }
    }

    void changeGunStats()
    {
        //set current gun stats to selected gun, the update UI
        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;

        gunModel.GetComponent<MeshFilter>().mesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().material = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        updatePlayerUI();
    }

    public void fillAmmo()
    {
        for (int i = inventorySystem.inventory.items.Count - 1; i > 0; i--)
        {
            if (inventorySystem.inventory.items[i].typeID == 'a')
            {
                inventorySystem.inventory.items.Remove(inventorySystem.inventory.items[i]);
                gunList[selectedGun].currAmmo = gunList[selectedGun].maxAmmo;
                updatePlayerUI();
                break;
            }
        }

    }

    public void replaceBattery(float amount)
    {
        fLight.intensity += amount;
        gameManager.instance.save.fLightIntensity = fLight.intensity;
    }
}