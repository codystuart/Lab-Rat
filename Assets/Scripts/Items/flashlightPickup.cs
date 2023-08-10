using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlightPickup : MonoBehaviour
{
    [SerializeField] GameObject interact;

    void Update()
    {
        transform.Rotate(0, 70 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.pickupFlashlight();
            gameManager.instance.playerScript.hasFlashlight = true;
            gameObject.transform.position = new Vector3(0, -1, 0);
            StartCoroutine(showInteract());
        }
    }

    IEnumerator showInteract()
    {
        interact.SetActive(true);
        yield return new WaitForSeconds(2f);
        interact.SetActive(false);
        Destroy(gameObject);
    }
}