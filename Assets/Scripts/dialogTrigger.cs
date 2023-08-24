using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogTrigger : MonoBehaviour
{
    public static dialogTrigger dialog;
    public AudioSource ring;
    public string[] lines;

    void Start()
    {
        dialog = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(startDialog());
        }
    }

    IEnumerator startDialog()
    {
        ring.Play();
        yield return new WaitForSeconds(1f);
        gameManager.instance.doDialog();
        gameObject.SetActive(false);
    }
}