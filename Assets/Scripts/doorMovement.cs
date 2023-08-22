using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class doorMovement : MonoBehaviour
{
    private bool playerInRange;
    private bool doorIsOpen;
    [SerializeField] GameObject doorInteractText;
    [SerializeField] AudioSource doorOpenSound;

    private void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "Level 1")
        {
            if (playerInRange && Input.GetKeyDown("e"))
            {
                doorIsOpen = true;
                doorOpenSound.Play();
                hideText();
                transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
        }
        else
        {
            if (playerInRange && PhoneScript.phone.phonePickedUp && Input.GetKeyDown("e"))
            {
                doorIsOpen = true;
                doorOpenSound.Play();
                hideText();
                transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
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
