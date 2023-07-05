using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [Header ("----- Camera -----")]
    [SerializeField] int sensitivity;
    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;
    [SerializeField] bool invertY;

    //class objets
    private float xRotation;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        transform.rotation = Quaternion.LookRotation(new Vector3 (0, 0, 0));
    }


    void Update()
    {
        //get input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        if (invertY)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        //clamp the camera rotation on the x-axis
        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);

        //rotate the camera on x-axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //rotate the player on the y-axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}