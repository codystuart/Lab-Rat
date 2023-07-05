using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottleRotator : MonoBehaviour
{
    [SerializeField] Vector3 rotation;
    [SerializeField] float speed;

    private void Start()
    {
        gameManager.instance.totalCureCount += 1;
    }
    private void Update()
    {
        transform.Rotate(rotation * speed * Time.deltaTime, Space.World);
    }
}
