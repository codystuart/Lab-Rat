using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class KeyCardScript : MonoBehaviour
{
    //there is a lot of repeated logic in here that is in other scripts
    //the inventory has the objectives text
    //there is no need to turn the objective on and off as the inventory has that logic

    public TextMeshProUGUI objective;
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
            objective.text = "Search for supplies and find a way to exit.";
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            objective.text = "Escape from the hordes of zombies";
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            objective.text = "Use the rooms to\nhide from the zombies.\nWalk near objects to find the key.\nPress \"Y\" to turn\nthis message off and on.";
        }
        else if (SceneManager.GetActiveScene().name == "Level 4")
        {
            objective.text = "The rooms can't help now. Get out as soon as possible.";
        }
        else if (SceneManager.GetActiveScene().name == "Level 5")
        {
            objective.text = "Get to the helipad and escape with the cure.";
        }
    }

    void Update()
    {
        //DoorFunction(); if this isn't being used then it needs to be gotten rid of
        HasKeyCard();
        CheckPlayerDistance();
    }

    public void HasKeyCard()
    {
        if (PickedUpKeyCard == true && SceneManager.GetActiveScene().name != "Level 5")
        {
            objective.text = "Go to the door to exit.";
        }
        else if (PickedUpKeyCard == true && SceneManager.GetActiveScene().name == "Level 5")
        {
            objective.text = "I hope this cure is enough.";
        }
    }

    public void DoorFunction()
    {
        if (PickedUpKeyCard == true)
        {
            //exitDoor DoorControls = Door.GetComponent<exitDoor>();
            //gameManager.instance.
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