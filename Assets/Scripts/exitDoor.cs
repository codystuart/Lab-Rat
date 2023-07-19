using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitDoor : MonoBehaviour
{
    [SerializeField] GameObject exitPointLight;
    [SerializeField] GameObject exitSignColor;
    [SerializeField] Material greenLight;
    [SerializeField] GameObject exitDoorInteractText;
    [SerializeField] GameObject lockedFindKeycardText;
    [SerializeField] GameObject lockedFindCureText;
    [SerializeField] GameObject lockedClearAreaText;
    [SerializeField] AudioSource lockedDoor;
    private bool playerInRange;

    void Update()
    { 
        if (keycardAcquired())
        {
            changeLightColor();
        }
        if (playerInRange && playerCanExit() && Input.GetKeyDown("e"))
        {
            // When more levels are added
            // Load next scene
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Loading next level");

            //For now player will win because there's only one level at this time
            gameManager.instance.youWin();
        }
        else if (playerInRange && !playerCanExit() && Input.GetKeyDown("e"))
        {
            lockedDoor.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
            showText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            hideText();
        }
    }
    void changeLightColor()
    {
        exitPointLight.GetComponent<Light>().color = Color.green;
        MeshRenderer material = exitSignColor.GetComponent<MeshRenderer>();
        material.material = greenLight;
    }

    bool keycardAcquired()
    {
        if (gameManager.instance.keycardAcquired == true)
            return true; 
        else 
            return false;
    }

    bool cureCollected()
    {
        if (gameManager.instance.collectedAllCures == true)
            return true;
        else
            return false;
    }

    bool zombiesEliminated()
    {
        if (gameManager.instance.enemiesRemaining <= 0)
            return true;
        else
            return false;
    }    
    
    bool playerCanExit()
    {
        if (keycardAcquired() && cureCollected() && zombiesEliminated())
            return true;
        else return false;
    }

    void showText()
    {
        if (!keycardAcquired())
        {
            lockedFindKeycardText.SetActive(true);
        }
        else if (!cureCollected())
        {
            lockedFindCureText.SetActive(true);
        }
        else if (!zombiesEliminated())
        {
            lockedClearAreaText.SetActive(true);
        }
        else if (playerCanExit())
        {
            exitDoorInteractText.SetActive(true);
        }
    }

    void hideText()
    {
        if (!keycardAcquired())
        {
            lockedFindKeycardText.SetActive(false);
        }
        else if (!cureCollected())
        {
           lockedFindCureText.SetActive(false);
        }
        else if (!zombiesEliminated())
        {
            lockedClearAreaText.SetActive(false);
        }
        else if (playerCanExit())
        {
            exitDoorInteractText.SetActive(false);
        }
    }
    
}