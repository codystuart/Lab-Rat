using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class exitDoor : MonoBehaviour
{
    private bool playerInRange;
    [SerializeField] GameObject doorInteractText;
    [SerializeField] GameObject lockedText;
    [SerializeField] AudioSource doorOpenSound;
    [SerializeField] AudioSource lockedDoorSound;
    public string sceneName;

    //objects to change color of light
    [SerializeField] GameObject exitPointLight;
    [SerializeField] GameObject exitSignColor;
    [SerializeField] Material greenLight;

    void Update()
    {
        if (keycardAcquired())
        {
            changeLightColor();
        }
        if (playerInRange && playerCanExit() && Input.GetKeyDown(KeyCode.E))
        {
            // If last level player wins
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                gameManager.instance.youWin();
            }
            else // If more levels, load next level
            {

                //remove old keycard before switching scenes
                for (int i = 0; i < inventorySystem.inventory.items.Count; ++i)
                {
                    if (inventorySystem.inventory.items[i].typeID == 'k')
                    {
                        inventorySystem.inventory.removeItem(inventorySystem.inventory.items[i]);
                    }
                }

                //save guns, items, and notes to bring to the next level
                gameManager.instance.save.saveGunList = gameManager.instance.playerScript.gunList;
                gameManager.instance.save.saveInvItems = inventorySystem.inventory.items;
                gameManager.instance.save.saveNotes = inventorySystem.inventory.notes;

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            }
        }
        else if (playerInRange && !playerCanExit() && Input.GetKeyDown(KeyCode.E))
        {
            lockedDoorSound.Play();
            lockedText.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;

            if (playerCanExit())
                doorInteractText.SetActive(true);
            else
                lockedText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            doorInteractText.SetActive(false);
            lockedText.SetActive(false);
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
        return gameManager.instance.keycardAcquired;
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
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Level 1")
        {
            //forces player to pick up phone, flashlight, and keycard to continue
            if (keycardAcquired() && gameManager.instance.playerScript.hasFlashlight
                && gameManager.instance.playerScript.hasPhone)
            {
                //SceneManager.LoadScene("Level 2");
                KeyCardScript.PickedUpKeyCard = false;
                return true;
            }
            else return false;
        }
        else if (sceneName == "Level 2")
        {
            if (keycardAcquired())
            {
                //SceneManager.LoadScene("Level 3");
                KeyCardScript.PickedUpKeyCard = false;
                return true;
            }
            else return false;
            //if (keycardAcquired() && cureCollected() && zombiesEliminated())
            //    return true;
            //else return false;
        }
        else if (sceneName == "Level 3")
        {
            return gameManager.instance.cureCollected;
            //if (keycardAcquired() && cureCollected() && zombiesEliminated())
            //    return true;
            //else return false;
        }
        //else if (sceneName == "Level 4")
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
        //else if (sceneName == "Level 5")
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
}