using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [Range(1, 10)][SerializeField] int damage;
    [SerializeField] int speed;
    [Range(1, 10)][SerializeField] int destroyTime;

    void Start()
    {
        Destroy(gameObject, destroyTime);
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();

        if (damageable != null)
            damageable.TakeDamage(damage);

        Destroy(gameObject);
    }
}