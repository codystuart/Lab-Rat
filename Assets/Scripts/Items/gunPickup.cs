using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] GameObject selfReference;
    [SerializeField] gunStats gun;
    [Range(70, 100)][SerializeField] int rotationSpeed;

    private void Start()
    {
        selfReference.layer = LayerMask.NameToLayer("Default");
        gun.currAmmo = gun.maxAmmo;
    }

    void Update()
    {
        //rotates the gun
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventorySystem.inventory.pickupSound.Play();
            gameManager.instance.playerScript.gunPickup(gun);
            selfReference.layer = LayerMask.NameToLayer("HandHeldItems");
            Destroy(gameObject);
        }
    }
}