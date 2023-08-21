using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneScript : MonoBehaviour
{
    public static PhoneScript phone;
    public string[] lines;

    void Start()
    {
        phone = this;
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
