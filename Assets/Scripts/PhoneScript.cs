using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneScript : MonoBehaviour
{
    public static PhoneScript phone;
    public string[] lines;
    public AudioSource ring;
    public bool phonePickedUp;

    void Start()
    {
        phone = this;
        phonePickedUp = false;
        StartCoroutine(loopRing());
    }

    private void OnTriggerEnter(Collider other)
    {
        //phone simpy start the dialog for level 1
        if (other.CompareTag("Player"))
        {
            phonePickedUp = true;
            Destroy(transform.gameObject);
            gameManager.instance.playerScript.hasPhone = true;
            gameManager.instance.doDialog();
        }
    }

    IEnumerator loopRing()
    {
        float length = ring.clip.length;
        
        while (!phonePickedUp)
        {
            ring.Play();
            yield return new WaitForSeconds(length + 1.5f);
        }
    }
}
