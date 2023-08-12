using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class firstSelected : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //sets the first selected button of each menu
    [SerializeField] GameObject first;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(first);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(first);
    }
}