using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    [SerializeField] GameObject creditScreen;
    public void Back()
    {
        creditScreen.SetActive(false);
    }
}
