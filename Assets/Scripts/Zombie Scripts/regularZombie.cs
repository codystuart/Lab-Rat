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
    [Range(10, 360)][SerializeField] int viewAngle = 90;
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

    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
    }

    void Update()
    {
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
        Debug.Log(angleToPlayer);

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
                    if(!isHitting)
                    {
                        StartCoroutine(dealDamage());
                    }
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
        gameManager.instance.player.GetComponent<playerController>().TakeDamage(damage);

        // Knock back the player by a space
        //gameManager.instance.player.transform.position =
        //    new Vector3(gameManager.instance.player.transform.position.x, gameManager.instance.player.transform.position.y, (gameManager.instance.player.transform.position.z - 1f));

        yield return new WaitForSeconds(1f);
        isHitting = false;
    }

}
