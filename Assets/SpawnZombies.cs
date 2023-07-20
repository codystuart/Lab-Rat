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
    public static bool CrawlSpawn;
    public static bool RegularSpawn;
    public static bool SpitterSpawn;
    public static bool TankSpawn;
    public int CrawlCount;
    public int RegularCount;
    public int SpitterCount;
    public int TankCount;
    // Start is called before the first frame update
    void Start()
    {
        CrawlSpawn = false;
        RegularSpawn = false;
        SpitterSpawn = false;
        TankSpawn = false;

        CrawlCount = 0;
        RegularCount = 0;
        SpitterCount = 0;
        TankCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (CrawlSpawn == true && CrawlingZombieSpawnPoint != null && CrawlCount == 0)
        {//If SpawnCrawl is true and the Zombie and spawn point are not empty
            CrawlCount++;
            Instantiate(CrawlingZombie,CrawlingZombieSpawnPoint.transform.position,Quaternion.identity);//Create zombie at spawn point
            Debug.Log("Respawn detected. Respawning Crawler Zombie");
            CrawlSpawn = false;//Set bool variable back to false
        }
        if (RegularSpawn == true && RegularZombieSpawnPoint != null&&RegularCount==0)
        {//If SpawnCrawl is true and the Zombie and spawn point are not empty
            RegularCount++;
            Instantiate(RegularZombie, RegularZombieSpawnPoint.transform.position, Quaternion.identity);//Create zombie at spawn point
            Debug.Log("Respawn detected. Respawning Regular Zombie");
            RegularSpawn = false;//Set bool variable back to false
        }
        if (SpitterSpawn == true && SpitterZombieSpawnPoint != null&&SpitterCount==0)
        {//If SpawnCrawl is true and the Zombie and spawn point are not empty
            SpitterCount++;
            Instantiate(SpitterZombie, SpitterZombieSpawnPoint.transform.position, Quaternion.identity);//Create zombie at spawn point
            Debug.Log("Respawn detected. Respawning Spitter Zombie");
            SpitterSpawn = false;//Set bool variable back to false
        }
        if (TankSpawn == true && TankZombieSpawnPoint != null&&TankCount==0)
        {//If SpawnCrawl is true and the Zombie and spawn point are not empty
            TankCount++;
            Instantiate(TankZombie, TankZombieSpawnPoint.transform.position, Quaternion.identity);//Create zombie at spawn point
            Debug.Log("Respawn detected. Respawning Tank Zombie");
            TankSpawn = false;//Set bool variable back to false
        }
    }
}
