using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class doorMovement : MonoBehaviour
{
    private bool playerInRange;
    private bool doorIsOpen;
    [SerializeField] GameObject doorInteractText;
    [SerializeField] GameObject lockedText;
    [SerializeField] AudioSource doorOpenSound;
    public string sceneName;

    private void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;
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
        sceneName = SceneManager.GetActiveScene().name;

        if (other.CompareTag("Player")) 
        {
            playerInRange = true;

            if (sceneName != "Level 1")
            {
                if (!doorIsOpen)
                    showText();
            }
            else if (!PhoneScript.phone.phonePickedUp && !doorIsOpen)
            {
                lockedText.SetActive(true);
            }
            else
            {
                if (!doorIsOpen)
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
        lockedText.SetActive(false);
    }
}
