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

    void Start()
    {

    }

    void Update()
    {

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
}

