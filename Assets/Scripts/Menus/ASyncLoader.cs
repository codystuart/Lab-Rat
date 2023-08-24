using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
    [SerializeField] Canvas loadingScreen;

    [SerializeField] Slider loadingBar;

    [SerializeField] GameObject continueText, loadingText;

    public void Play(string levelToLoad)
    {
        loadingScreen.sortingOrder = 3;
        
        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        yield return null;

        // Begin loading scene
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        // Don't activate scene until allowed
        loadOperation.allowSceneActivation = false;
        
        // While loading is in progress, update progress bar
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingBar.value = progressValue;
            
            // If loading has finished
            if(loadOperation.progress >= 0.9f)
            {
                // Hide "loading" text and show "continue" text
                loadingText.SetActive(false);
                continueText.SetActive(true);
                // Wait until any button is pressed
                if (Input.anyKeyDown)
                    // Activate scene
                    loadOperation.allowSceneActivation = true;
            }
            yield return null;
        }


    }
}
