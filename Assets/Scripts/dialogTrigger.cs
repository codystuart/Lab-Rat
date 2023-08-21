using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogTrigger : MonoBehaviour
{
    public static dialogTrigger dialog;
    public string[] lines;

    void Start()
    {
        dialog = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.doDialog();
            gameObject.SetActive(false);
        }
    }
}