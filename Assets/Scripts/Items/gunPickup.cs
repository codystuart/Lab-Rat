using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gunPickup : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] GameObject selfReference;
    [SerializeField] gunStats gun;

    private void Start()
    {
        //default is for the main camera
        selfReference.layer = LayerMask.NameToLayer("Default");

        //set ammo to max only on first level
        Scene sceneCurr = SceneManager.GetActiveScene();
        if (sceneCurr.name == "Level 1")
            gun.currAmmo = gun.maxAmmo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventorySystem.inventory.pickupSound.Play();
            gameManager.instance.playerScript.gunPickup(gun);
            //gameManager.instance.save.saveGunList.Add(gun);
            selfReference.layer = LayerMask.NameToLayer("HandHeldItems"); //handhelditems is for the items camera
            Destroy(gameObject);
        }
    }
}