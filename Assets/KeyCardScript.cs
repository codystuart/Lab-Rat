using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Objectives.text = "Pick up the gun and cellphone.\nThen find the keycard to exit.";
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
