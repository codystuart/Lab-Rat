using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VaseScript : MonoBehaviour
{
    public GameObject player;
    public GameObject vase;
    public GameObject KeyCard;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if (KeyCard != null)
        {
            KeyCard.SetActive(false);
        }
        vase.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerDistance();
    }
    public float GetDistance(Vector3 obj1, Vector3 obj2)
    {
        return Vector3.Distance(obj1, obj2);
    }

    public void CheckPlayerDistance()
    {
        float distance = GetDistance(transform.position, player.transform.position);
        if (distance <= 2 && SceneManager.GetActiveScene().name == "Level 3")
        {
            vase.SetActive(true);
            if (KeyCard != null)
            {
                KeyCard.SetActive(true);
                KeyCard.GetComponent<Collider>().enabled = true;
            }
            transform.gameObject.SetActive(false);

        }
    }

    
}
