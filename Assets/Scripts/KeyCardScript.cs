using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class KeyCardScript : MonoBehaviour
{
    public static bool PickedUpKeyCard;
    public bool DirectionsOn;
    public GameObject Door;
    public GameObject player;

    void Start()
    { 
        player = gameManager.instance.player;
        PickedUpKeyCard = false; 
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            gameManager.instance.objectiveText.text = "Search for supplies and find a way to exit.";
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            gameManager.instance.objectiveText.text = "Clear the area.";
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            gameManager.instance.objectiveText.text = "Escape from the hordes of zombies";
        }
        //else if (SceneManager.GetActiveScene().name == "Level 4")
        //{
        //    gameManager.instance.objectiveText.text = "Find the cure and leave.";
        //}
        //else if (SceneManager.GetActiveScene().name == "Level 5")
        //{
        //    gameManager.instance.objectiveText.text = "Get to the helipad and escape with the cure.";
        //}
    }

    void Update()
    {
        HasKeyCard();
        CheckPlayerDistance();
    }

    public void HasKeyCard()
    {
        if (PickedUpKeyCard == true && SceneManager.GetActiveScene().name != "Level 5")
        {
            gameManager.instance.objectiveText.text = "Go to the door to exit.";
        }
        else if (PickedUpKeyCard == true && SceneManager.GetActiveScene().name == "Level 5")
        {
            gameManager.instance.objectiveText.text = "I hope this cure is enough.";
        }
    }
    
    public float GetDistance(Vector3 obj1, Vector3 obj2)
    {
       return Vector3.Distance(obj1, obj2);
    }

    public void CheckPlayerDistance()
    {
        float distance = GetDistance(transform.position, player.transform.position);
        if (distance <= 10 && SceneManager.GetActiveScene().name == "Level 5")
        {
            PickedUpKeyCard = true;
        } 
    }
}