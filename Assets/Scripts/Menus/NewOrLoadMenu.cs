using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewOrLoadMenu : MonoBehaviour
{
    [SerializeField] Canvas newOrLoadUI;
    public void Back()
   {
        // hide new game screen by setting sorting order to 0;
        newOrLoadUI.sortingOrder = 0;
   }
}
