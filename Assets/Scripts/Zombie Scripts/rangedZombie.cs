using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class rangedZombie : MonoBehaviour, IDamage
{
    [Header("Components")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Material material;
    [SerializeField] Image hpBar;
    [SerializeField] GameObject enemyUI;

    [Header("Crawler Zombie Stats")]
    [Range(1,10)][SerializeField] int HP;
    [SerializeField] GameObject itemDrop;
    //[SerializeField] int damage;

    [Header("Regular Zombie Navigation")]
    [Range(10, 360)][SerializeField] int viewAngle = 90;
    [Range(1, 8)][SerializeField] int playerFaceSpeed = 6;
    [SerializeField] int roamTimer = 5;
    [SerializeField] int roamDist = 10;

    [Header("Spitball Stats")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject spitBall;
    [SerializeField] Transform shootPos;

    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    float stoppingDistanceOrig;
    Vector3 startingPos;
    bool destinationChosen;
    private bool isShooting;
    private float originalHP;
    public GameObject Zombie;
    public AnimationClip[] AnimsArray;
    Animation animator;

    void Start()
    {
        originalHP = HP;
        gameManager.instance.updateGameGoal(1);
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
    }

    void Update()
    {
        enemyUI.transform.LookAt(gameManager.instance.player.transform.position);

        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (agent.destination != gameManager.instance.player.transform.position)
            StartCoroutine(roam());
    }

    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            StopAnimation();
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTimer);

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
        else
        {

            PlayZombieAnim("idle");
        }
    }

    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    bool canSeePlayer()
    {
        agent.stoppingDistance = stoppingDistanceOrig;
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer < viewAngle)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    facePlayer();
                }

                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    public void TakeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashDamage());
        updateEnemyUI();

        if (HP <= 0)
        {
            if (itemDrop != null)
            {
                Instantiate(itemDrop, transform.position , Quaternion.identity);
            }
            Destroy(gameObject);
            gameManager.instance.updateGameGoal(-1);
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material = material;
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(spitBall, shootPos.position, transform.rotation);
        Debug.Log("Should play attack animation.");
        PlayZombieAnim("attack");
        //Animator anim = Zombie.GetComponent<Animator>();
        //if (anim != null)
        //{ 
        //    Debug.Log("Should play Zombie Anim");
        //}
        //else if (anim == null)
        //{
        //    Debug.Log("Can't find anim");
        //}
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }

    public void PlayZombieAnim(string AnimName)
    {
        if (Zombie != null)
        {
            animator = Zombie.GetComponent<Animation>();
            if (animator == null)
            {
                Debug.Log("Can't find animator.");
            }
            else if (animator != null)
            {
                if (AnimsArray.Length == 0)
                {
                    Debug.Log("Can't find animations.");
                }
                else if (AnimsArray.Length > 0)
                {
                    for (int i = 0; i < AnimsArray.Length; i++)
                    {
                        if (AnimName.ToLower() == AnimsArray[i].name.ToLower())
                        { 
                            animator.clip = AnimsArray[i];
                            animator.Play();
                        }
                    }
                }
            }
        }
    }

    public void StopAnimation()
    {
        if (animator != null && animator.isPlaying == true)
        {
            animator.Stop();
        }
    }

    public void updateEnemyUI()
    {
        hpBar.fillAmount = (float)HP / originalHP;
    }
}