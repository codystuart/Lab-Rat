using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueBtn : MonoBehaviour
{   
    [SerializeField] bool playerInRange;
    public GameObject blueButton;
    [SerializeField] GameObject buttonInteractText;
    private void Update()
    {
     if (playerInRange && Input.GetKeyDown("e"))
        {
            gameManager.instance.ButtonPressedOrder(blueButton);
            Debug.Log("Blue btn pressed");

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
            if (gameManager.instance.correctOrder == false)
            {
                showText();
            }
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
    void showText()
    {
        buttonInteractText.SetActive(true);
    }
    void hideText()
    {
        buttonInteractText.SetActive(false);
    }
}
