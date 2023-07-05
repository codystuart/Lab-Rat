using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedZombie : MonoBehaviour, IDamage
{
    [Header("Components")]
    [SerializeField] Renderer model;

    [Header("Ranged Zombie Stats")]
    [SerializeField] int hp = 10;
    [SerializeField] int speed = 7;
    [SerializeField] int damage;

    [Header("Spitball Stats")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject spitBall;
    [SerializeField] Transform shootPos;

    private bool playerInRange;
    private bool isShooting;
    

    void Start()
    {

    }

    void Update()
    {
        if (playerInRange) 
        {
            StartCoroutine(shoot());  
        }

    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        StartCoroutine(flashDamage());

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(spitBall, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
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

}

