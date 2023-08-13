using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dialog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] GameObject continueTag;
    public string[] lines;
    public float textSpeed;

    private int index;

    void Start()
    {
        //set text to empty then start dialog
        dialogText.text = string.Empty;
        continueTag.SetActive(false);
        startDialog();
    }

    void Update()
    {
        //do something if the player presses E
        if(Input.GetKeyDown(KeyCode.E))
        {
            continueTag.SetActive(false);

            if (dialogText.text == lines[index])
                nextLine();
            else
            {
                StopAllCoroutines();
                dialogText.text = lines[index];
                continueTag.SetActive(true);
            }
        }
    }

    void startDialog()
    {
        //set index to first line then display text
        index = 0;
        StartCoroutine(typeLine());
    }

    IEnumerator typeLine()
    {
        //displays text one character at a time
        foreach (char c in lines[index].ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        continueTag.SetActive(true);
    }

    void nextLine()
    {
        //goes to next line
        //sets inactive when no lines left
        if (index < lines.Length - 1)
        {
            ++index;
            dialogText.text = string.Empty;
            StartCoroutine(typeLine());
        }
        else
        {
            gameManager.instance.playerScript.canMove = true;
            gameManager.instance.stateUnpaused();
        }
    }
}