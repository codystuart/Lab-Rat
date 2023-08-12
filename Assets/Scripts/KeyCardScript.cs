using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyCardScript : MonoBehaviour
{
    public Text Objectives;
    public static bool PickedUpKeyCard;
    public bool DirectionsOn;
    public GameObject Door;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            Objectives.text = "Pick up the gun and cellphone.\nThen find the keycard to exit.\nPress \"Y\" to turn\nthis message off and on.";
        }
        else if (SceneManager.GetActiveScene().name == "Level 2" || SceneManager.GetActiveScene().name == "Level 3")
        {
            Objectives.text = "Use the rooms to\nhide from the zombies.\nPress \"Y\" to turn\nthis message off and on.";
        }
        else if (SceneManager.GetActiveScene().name == "Level 4")
        {
            Objectives.text = "The rooms can't help now. Get out as soon as possible.\nPress \"Y\" to turn\nthis message off and on.";
        }
        else if (SceneManager.GetActiveScene().name == "Level 5")
        {
            Objectives.text = "Get to the helipad and escape with the cure.\nPress \"Y\" to turn\nthis message off and on.";
        }
        DirectionsOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        DoorFunction();
        TurnOffDirections();
        HasKeyCard();
    }

    public void TurnOffDirections()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            DirectionsOn = !DirectionsOn;
            if (DirectionsOn == true)
            {
                Objectives.enabled = true;
            }
            else if (DirectionsOn == false)
            {
                Objectives.enabled = false;
            }
        } 
    }

    public void HasKeyCard()
    {
        if (PickedUpKeyCard == true)
        {
            Objectives.text = "Go to the door to exit.";
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
}
