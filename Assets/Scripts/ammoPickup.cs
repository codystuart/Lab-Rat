using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickup : MonoBehaviour
{
    [Header("----- Rotation Speed -----")]
    [Range(70, 100)][SerializeField] int rotationSpeed;

    void Update()
    {
        //rotates ammo box
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.instance.playerScript.gunList.Count < 0)
        {
            StartCoroutine(noGun());
        }
        else if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.ammoPickup();
            Destroy(gameObject);
        }
    }

    IEnumerator noGun()
    {
        gameManager.instance.noGun.SetActive(true);
        yield return new WaitForSeconds(2);
        gameManager.instance.noGun.SetActive(false);
    }
}