using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    [SerializeField] Canvas creditsUI;
    public void Back()
    {
        // hide options by setting sorting order to 0;
        creditsUI.sortingOrder = 0;
    }
}
