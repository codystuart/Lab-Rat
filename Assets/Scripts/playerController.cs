using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class playerController : MonoBehaviour
{
    [Header("----- Player -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(1, 100)][SerializeField] int hp;
    [Range(1, 10)][SerializeField] float playerSpeed;
    [Range(1,3)] [SerializeField] int sprintDuration;
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
    private int jumpCount;
    private bool playerGrounded;
    private bool isShooting;
    private float playerSpeedOrig;

    private void Start()
    {
        playerSpeedOrig = playerSpeed;
    }

    private void Update()
    {
        Movement();

        if (Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
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

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    IEnumerator sprint()
    {
        playerSpeed = 10;
        yield return new WaitForSeconds(sprintDuration);
        playerSpeed = playerSpeedOrig;

    }
}