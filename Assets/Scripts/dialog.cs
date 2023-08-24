using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class dialog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] GameObject continueTag;
    public float textSpeed;

    string sceneName;
    int index;

    void Start()
    {
        //set text to empty then start dialog
        dialogText.text = string.Empty;
        continueTag.SetActive(false);
        sceneName = SceneManager.GetActiveScene().name;
        startDialog();
    }

    void Update()
    {
        if (sceneName != "Level 1")
        {
            //do something if the player presses E
            if (Input.GetKeyDown(KeyCode.E))
            {
                continueTag.SetActive(false);

                if (dialogText.text == dialogTrigger.dialog.lines[index])
                    nextLine();
                else
                {
                    StopAllCoroutines();
                    dialogText.text = dialogTrigger.dialog.lines[index];
                    continueTag.SetActive(true);
                }
            }
        }
        else
        {
            //do something if the player presses E
            if (Input.GetKeyDown(KeyCode.E))
            {
                continueTag.SetActive(false);

                if (dialogText.text == PhoneScript.phone.lines[index])
                    nextLine();
                else
                {
                    StopAllCoroutines();
                    dialogText.text = PhoneScript.phone.lines[index];
                    continueTag.SetActive(true);
                }
            }
        }
    }

    void startDialog()
    {
        //set index to first line then display text
        index = 0;
        if (sceneName != "Level 1")
            StartCoroutine(dialogTypeLine());
        else
            StartCoroutine(phoneTypeLine());
    }

    IEnumerator dialogTypeLine()
    {
        //displays text one character at a time
        foreach (char c in dialogTrigger.dialog.lines[index].ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        continueTag.SetActive(true);
    }

    IEnumerator phoneTypeLine()
    {
        //displays text one character at a time
        foreach (char c in PhoneScript.phone.lines[index].ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        continueTag.SetActive(true);
    }

    void nextLine()
    {
        //goes to next line
        //sets inactive when no lines left
        if (sceneName != "Level 1")
        {
            if (index < dialogTrigger.dialog.lines.Length - 1)
            {
                ++index;
                dialogText.text = string.Empty;
                StartCoroutine(dialogTypeLine());
            }
            else
            {
                gameManager.instance.activeMenu.SetActive(false);
                gameManager.instance.stateUnpaused();
            }
        }
        else
        {
            if (index < PhoneScript.phone.lines.Length - 1)
            {
                ++index;
                dialogText.text = string.Empty;
                StartCoroutine(phoneTypeLine());
            }
            else
            {
                gameManager.instance.activeMenu.SetActive(false);
                gameManager.instance.stateUnpaused();
            }
        }
    }
}