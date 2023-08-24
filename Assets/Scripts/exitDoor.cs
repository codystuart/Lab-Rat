using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            // If last level player wins
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                gameManager.instance.youWin();
            }
            else // If more levels, load next level
            {
                gameManager.instance.save.saveGunList = gameManager.instance.playerScript.gunList;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Debug.Log("Loading next level");

            }
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
            //showText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            //hideText();
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

    // bool cureCollected()
    // {
    //     if (gameManager.instance.collectedAllCures == true)
    //         return true;
    //     else
    //         return false;
    // }

    // bool zombiesEliminated()
    // {
    //     if (gameManager.instance.enemiesRemaining <= 0)
    //         return true;
    //     else
    //         return false;
    // }    
    
    bool playerCanExit()
    { 
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            //forces player to pick up phone, flashlight, and first keycard to continue
            if (keycardAcquired() && gameManager.instance.playerScript.hasFlashlight
                && gameManager.instance.playerScript.hasPhone)
            {
                Debug.Log("Door will open.");
                SceneManager.LoadScene("Level 2");
                KeyCardScript.PickedUpKeyCard = false;
                return true;
            }
            else return false;
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            if (keycardAcquired())
            {
                Debug.Log("Door will open.");
                SceneManager.LoadScene("Level 3");
                KeyCardScript.PickedUpKeyCard = false;
                return true;
            }
            else return false;
            //if (keycardAcquired() && cureCollected() && zombiesEliminated())
            //    return true;
            //else return false;
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            if (keycardAcquired())
            {
                Debug.Log("Door will open.");
                //SceneManager.LoadScene("Level 4");
                KeyCardScript.PickedUpKeyCard = false;
                return true;
            }
            else return false;
            //if (keycardAcquired() && cureCollected() && zombiesEliminated())
            //    return true;
            //else return false;
        }
        //else if (SceneManager.GetActiveScene().name == "Level 4")
        //{
        //    if (keycardAcquired())
        //    {
        //        Debug.Log("Door will open.");
        //        SceneManager.LoadScene("Level 5");
        //        KeyCardScript.PickedUpKeyCard = false;
        //        return true;
        //    }
        //    else return false;
        //    //if (keycardAcquired() && cureCollected() && zombiesEliminated())
        //    //    return true;
        //    //else return false;
        //}
        //else if (SceneManager.GetActiveScene().name == "Level 5")
        //{
        //    if (keycardAcquired())
        //    {
        //        if(Objectives != null)
        //        Objectives.text = "I hope this cure is enough.";
        //        return true;
        //    }
        //    else return false;
        //    //if (keycardAcquired() && cureCollected() && zombiesEliminated())
        //    //    return true;
        //    //else return false;
        //}
        return false;
    }

    // void showText()
    // {
    //     if (!keycardAcquired())
    //     {
    //         lockedFindKeycardText.SetActive(true);
    //     }
    //     else if (!cureCollected())
    //     {
    //         lockedFindCureText.SetActive(true);
    //     }
    //     else if (!zombiesEliminated())
    //     {
    //         lockedClearAreaText.SetActive(true);
    //     }
    //     else if (playerCanExit())
    //     {
    //         exitDoorInteractText.SetActive(true);
    //     }
    // }

    // void hideText()
    // {
    //     if (!keycardAcquired())
    //     {
    //         lockedFindKeycardText.SetActive(false);
    //     }
    //     else if (!cureCollected())
    //     {
    //        lockedFindCureText.SetActive(false);
    //     }
    //     else if (!zombiesEliminated())
    //     {
    //         lockedClearAreaText.SetActive(false);
    //     }
    //     else if (playerCanExit())
    //     {
    //         exitDoorInteractText.SetActive(false);
    //     }
    // }
    
}
