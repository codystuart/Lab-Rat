using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneScript : MonoBehaviour
{
    [Header("----- Components -----")] 
    [Range(70, 100)] [SerializeField] int rotationSpeed;

    void Update()
    {
        //rotates the phone
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    { 
        //phone simpy start the dialog for level 1
        if (other.CompareTag("Player"))
        { 
            Destroy(transform.gameObject);
            gameManager.instance.playerScript.hasPhone = true;
            gameManager.instance.doDialog();
        }
    }
}
