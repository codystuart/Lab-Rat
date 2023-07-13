using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glass : MonoBehaviour, IDamage
{
    [SerializeField] int hp;

    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
