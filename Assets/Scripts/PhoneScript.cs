using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneScript : MonoBehaviour
{
    [Header("----- Components -----")] 
    [Range(70, 100)] [SerializeField] int rotationSpeed;

    private void Start()
    {
        rotationSpeed = 50;
    }

    void Update()
    {
        //rotates the gun
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.tag == ("Player") && this.gameObject.name.Contains("Key_cards"))
        {
            gameManager.instance.keycardAcquired = true;
            //Debug.Log("Should be able to open the door.");
            KeyCardScript.PickedUpKeyCard = true;
            Destroy(transform.gameObject);
        }
        else if (collision.gameObject.tag == ("Player"))
        { 
            Destroy(transform.gameObject);
        }
    }
}
