using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UI;


public class regularZombie : MonoBehaviour, IDamage
{
    [Header("---- Components ----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Material material;
    //[SerializeField] Animation anim;
    [SerializeField] GameObject enemyUI;
    [SerializeField] Image hpBar;
    [Range(1, 10)][SerializeField] int hideHP;

    [Header("---- Regular Zombie Stats ----")]
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 10)][SerializeField] int damage;

    [Header("---- Regular Zombie Navigation ----")]
    [Range(10, 50)][SerializeField] int viewAngle = 90;
    [Range(1, 8)][SerializeField] int playerFaceSpeed = 8;
    [SerializeField] int roamTimer = 3;
    [SerializeField] int roamDist = 10;

    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    float stoppingDistanceOrig;
    Vector3 startingPos;
    bool destinationChosen;
    private bool isHitting;
    private float originalHP;

    [Header("---- Animations ----")]
    [SerializeField] GameObject Zombie;
    [SerializeField] AnimationClip[] AnimsArray;
    [SerializeField] Animation animator;


    void Start()
    {
        originalHP = HP;
        gameManager.instance.updateGameGoal(1);
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
        enemyUI.SetActive(false);
        PlayZombieAnim("idle");
    }

    void Update()
    {
        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (agent.destination != gameManager.instance.player.transform.position)
            StartCoroutine(roam());


        enemyUI.transform.LookAt(gameManager.instance.player.transform.position);

        //float PlayerDistance = GetDistance(transform.position, gameManager.instance.player.transform.position);
        //if (gameManager.instance.player != null && PlayerDistance <= 2)
        //{
        //    PlayZombieAnim("attack2");
        //}
        //else if (gameManager.instance.player != null && PlayerDistance > 5)
        //{
        //    if (anim != null)
        //    {
        //        anim.Stop();
        //    }
        //    PlayZombieAnim("idle");
        //    Debug.Log("Playing idle. Distance: "+PlayerDistance);
        //}
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

    public void PlayZombieAnim(string AnimName)
    {
  
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
                if (animator.isPlaying != true)
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
        

        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer < viewAngle)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                // If the agent reached their stopping distance
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    facePlayer();
                    
                    // Deal damage to the player
                    if (!isHitting)
                    {
                        animator.Stop(); 
                        StartCoroutine(dealDamage()); 
                    }
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    } 
    

    private void OnCollisionEnter(Collision collision)//When player gets in range, collision method will not allow the player to walk through the zombie. It will solid
    {//Method will activate when another object touches/collides with this one.
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(transform.gameObject.name + " is in range of " + collision.gameObject.name);
            playerInRange = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(transform.gameObject.name+" is in range of "+other.name);
            playerInRange = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(transform.gameObject.name + " is not in range of " + other.name);
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
            Destroy(gameObject);
            gameManager.instance.updateGameGoal(-1);
        }
    }

    public void updateEnemyUI()
    {
        enemyUI.SetActive(true);
        hpBar.fillAmount = (float)HP / originalHP;
        StartCoroutine(showHealth());
    }

    IEnumerator showHealth()
    {
        yield return new WaitForSeconds(hideHP);
        enemyUI.SetActive(false);
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material = material;
    }

    IEnumerator dealDamage()
    { 
        isHitting = true;
        gameManager.instance.playerScript.TakeDamage(damage);

        // Knock back the player by a space
        //gameManager.instance.player.transform.position =
        //    new Vector3(gameManager.instance.player.transform.position.x, gameManager.instance.player.transform.position.y, (gameManager.instance.player.transform.position.z - 1f));
        
        yield return new WaitForSeconds(1f);
        isHitting = false;
    }

    public float GetDistance(Vector3 object1, Vector3 object2)
    {
        return Vector3.Distance(object1,object2);
    }
}