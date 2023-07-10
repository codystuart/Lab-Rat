using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class regularZombie : MonoBehaviour, IDamage
{
    [Header("Components")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Material material;

    [Header("Regular Zombie Stats")]
    [SerializeField] int hp = 5;
    [SerializeField] int damage = 10;

    [Header("Regular Zombie Navigation")]
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

    public GameObject Zombie;
    public AnimationClip[] AnimsArray;
    Animation animator;
    public GameObject player;

    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
        player = GameObject.Find("Player");
        PlayZombieAnim("attack2");
    }

    void Update()
    {
        //if (playerInRange && !canSeePlayer())
        //{
        //    StartCoroutine(roam());
        //}
        //else if (agent.destination != gameManager.instance.player.transform.position)
        //    StartCoroutine(roam());
        float PlayerDistance = GetDistance(transform.position, player.transform.position);
        if (player != null && PlayerDistance <= 2)
        {
            PlayZombieAnim("attack2");
        }
        else if (player != null && PlayerDistance > 5)
        {
            if (animator != null)
            {
                animator.Stop();
            }
            PlayZombieAnim("idle");
            Debug.Log("Playing idle. Distance: "+PlayerDistance);
        }
    }

    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
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
        hp -= amount;
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashDamage());

        if (hp <= 0)
        {
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