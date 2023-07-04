using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankZombie : MonoBehaviour, IDamage
{
    [Header("Components")]
    [SerializeField] Renderer model; 

    [Header("Tank Zombie Stats")]
    [SerializeField] int hp;
    [SerializeField] int speed;

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
