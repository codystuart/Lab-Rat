using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class firstSelected : MonoBehaviour
{
    //sets the first selected button of each menu
    [SerializeField] GameObject first;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(first);
    }
}