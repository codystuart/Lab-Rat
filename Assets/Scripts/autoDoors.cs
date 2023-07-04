using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class autoDoors : MonoBehaviour
{
    [SerializeField] GameObject door; //reference to door

    /*
    // door is a little janky
    // be sure to change values and move the doors/walls around as needed
    // if you know a better way to do this then please change it
     */
    public float maxOpenAmount = 10f;
    public float maxCloseAmount = 0f;
    public float speed = 15f;
    
    public bool playerNear;

    void Start()
    {
        playerNear = false;
    }


    void Update()
    {
        if (playerNear)
        {
            if (door.transform.position.x < maxOpenAmount)
            {
                door.transform.Translate(speed * Time.deltaTime, 0f,  0f);
            }
        }
        else
        {
            if (door.transform.position.x > maxCloseAmount)
            {
                door.transform.Translate(-speed * Time.deltaTime, 0f, 0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNear = false;
        }
    }
}
