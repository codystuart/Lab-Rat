using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class doorMovement : MonoBehaviour
{
    private bool playerInRange;
    private bool doorIsOpen;
    [SerializeField] GameObject doorInteractText;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown("e"))
        {
            Debug.Log("Door opens");
            doorIsOpen = true;
            hideText();
            transform.localRotation = Quaternion.Euler(0, 0, 90); 
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player")) 
        {
            playerInRange = true;
            if (!doorIsOpen)
            {
                showText();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = false;
            hideText();
        }
    }

    void showText()
    {
         doorInteractText.SetActive(true);
    }
    void hideText()
    {
        doorInteractText.SetActive(false);
    }
}
