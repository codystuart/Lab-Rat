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