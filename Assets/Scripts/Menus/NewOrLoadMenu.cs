using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewOrLoadMenu : MonoBehaviour
{
    [SerializeField] Canvas newOrLoadUI;
    public void Back()
   {
        // hide options by setting sorting order to 0;
        newOrLoadUI.sortingOrder = 0;
   }
}
