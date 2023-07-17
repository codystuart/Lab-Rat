using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] gunStats gun;
    [Range(70, 100)][SerializeField] int rotationSpeed;

    private void Start()
    {
        gun.currAmmo = gun.maxAmmo;
    }

    void Update()
    {
        //rotates the gun
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.gunPickup(gun);
            Destroy(gameObject);
        }
    }
}