using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ExplosiveItem : MonoBehaviour, IDamage
{
    [SerializeField] int hp;
    [SerializeField] float explosionRange = 4f;
    [SerializeField] int damageAmount;

    [SerializeField] GameObject explosionParticles;

    [Header("LayerMask for objects that can be exploded")]
    [SerializeField] LayerMask explodableLayerMask;
    [SerializeField] LayerMask playerLayerMask;

    // PLayer can shoot explosive objects that will explode objects in its range
    public void TakeDamage(int amount)
    {
        hp -= amount;
        if(hp <= 0)
        {
            Explode();
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            Destroy(explosionParticles, 1.5f);
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        Collider[] objectsToExplode = Physics.OverlapSphere(transform.position, explosionRange, explodableLayerMask);
        Collider[] playerToDamage = Physics.OverlapSphere(transform.position, explosionRange, playerLayerMask);

        // destroy any explodable objects in range
        foreach(var obj in objectsToExplode)
        {
            Destroy(obj.gameObject);
        }
        // if player is in range, damages player
        foreach(var obj in playerToDamage)
        {
            gameManager.instance.playerScript.TakeDamage(damageAmount);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
