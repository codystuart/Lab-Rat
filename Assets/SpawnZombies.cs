using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombies : MonoBehaviour
{
    public GameObject CrawlingZombie;
    public GameObject CrawlingZombieSpawnPoint;
    public GameObject RegularZombie;
    public GameObject RegularZombieSpawnPoint;
    public GameObject SpitterZombie;
    public GameObject SpitterZombieSpawnPoint;
    public GameObject TankZombie;
    public GameObject TankZombieSpawnPoint;
    public static bool SpawnCrawl;
    public static bool RegularCrawl;
    public static bool SpitterCrawl;
    public static bool TankCrawl;
    // Start is called before the first frame update
    void Start()
    {
        SpawnCrawl = false;
        RegularCrawl = false;
        SpitterCrawl = false;
        TankCrawl = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnCrawl == true && CrawlingZombie != null && CrawlingZombieSpawnPoint != null)
        {//If SpawnCrawl is true and the Zombie and spawn point are not empty
            Instantiate(CrawlingZombie,CrawlingZombieSpawnPoint.transform.position,Quaternion.identity);//Create zombie at spawn point
            Debug.Log("Respawn detected. Respawning Crawler Zombie");
            SpawnCrawl = false;//Set bool variable back to false
        }
        if (RegularCrawl == true && RegularZombie != null && RegularZombieSpawnPoint != null)
        {//If SpawnCrawl is true and the Zombie and spawn point are not empty
            Instantiate(RegularZombie, RegularZombieSpawnPoint.transform.position, Quaternion.identity);//Create zombie at spawn point
            Debug.Log("Respawn detected. Respawning Regular Zombie");
            RegularCrawl = false;//Set bool variable back to false
        }
        if (SpitterCrawl == true && SpitterZombie != null && SpitterZombieSpawnPoint != null)
        {//If SpawnCrawl is true and the Zombie and spawn point are not empty
            Instantiate(SpitterZombie, SpitterZombieSpawnPoint.transform.position, Quaternion.identity);//Create zombie at spawn point
            Debug.Log("Respawn detected. Respawning Spitter Zombie");
            SpitterCrawl = false;//Set bool variable back to false
        }
        if (TankCrawl == true && TankZombie != null && TankZombieSpawnPoint != null)
        {//If SpawnCrawl is true and the Zombie and spawn point are not empty
            Instantiate(TankZombie, TankZombieSpawnPoint.transform.position, Quaternion.identity);//Create zombie at spawn point
            Debug.Log("Respawn detected. Respawning Tank Zombie");
            TankCrawl = false;//Set bool variable back to false
        }
    }
}
