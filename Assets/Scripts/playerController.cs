using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class playerController : MonoBehaviour, IDamage, healthBottle
{
    [Header("----- Player -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(1, 50)][SerializeField] int HP;
    [Range(1, 10)][SerializeField] float playerSpeed;
    [Range(1, 5)] [SerializeField] int sprintDuration;
    [Range(5,10)][SerializeField] float jumpHeight;
    [SerializeField] float gravity;
    [SerializeField] int jumpMax;

    [Header("----- Gun Stats -----")]
    [Range(0.1f,1.0f)][SerializeField] float shootRate;
    [Range(1, 10)][SerializeField] int shootDamage;
    [Range(10, 100)][SerializeField] int shootDist;


    //class objects
    private Vector3 move;
    private Vector3 velocity;
    private int originalHP;
    private int jumpCount;
    private bool playerGrounded;
    private bool isShooting;
    private float playerSpeedOrig;


    private void Start()
    {
        originalHP = HP;
        playerSpeedOrig = playerSpeed;
        spawnPlayer();

    }

    private void Update()
    {
        Movement();

        if (Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("key pressed");
            StartCoroutine(sprint());
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
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            IDamage damageable= hit.collider.GetComponent<IDamage>();

            if(damageable != null)
            {
                damageable.TakeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(gameManager.instance.playerFlashDamage());
        updatePlayerUI();


        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
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
        playerSpeed = 10;
        yield return new WaitForSeconds(sprintDuration);
        playerSpeed = playerSpeedOrig;

    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHpBar.fillAmount = (float)HP / originalHP;
    }

    // Player can pick up cure bottles
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cure"))
        {
            other.gameObject.SetActive(false);
            gameManager.instance.updateCureGameGoal(1);
        }

        if (other.gameObject.CompareTag("HPBottle"))
        {
            other.gameObject.SetActive(false);
            giveHealth(50);
           
        }
    }

    public void giveHealth(int amount)
    {
        HP += amount;
        updatePlayerUI();
    }



}