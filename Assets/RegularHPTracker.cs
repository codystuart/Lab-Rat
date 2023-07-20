using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularHPTracker : MonoBehaviour
{
    public bool RespawnCrawlZombie;
    public bool RespawnRegularZombie;
    public bool RespawnTankZombie;
    public bool RespawnSpitterZombie;

    public GameObject CrawlZombie;
    public GameObject RegularZombie;
    public GameObject TankZombie;
    public GameObject SpitterZombie;
    // Start is called before the first frame update
    void Start()
    {
        RespawnCrawlZombie = false;
        RespawnRegularZombie = false;
        RespawnTankZombie = false;
        RespawnSpitterZombie = false;

        
    }



    public void PrintHPOfZombies()
    {
        if (CrawlZombie == null)
        {
            RespawnCrawlZombie = true;
            CrawlZombie = GameObject.Find("Crawler(Clone)"); 
        }
        if (RegularZombie == null)
        { 
            RespawnRegularZombie = true;
            RegularZombie = GameObject.Find("Regular Zombie(Clone)");
        }
        if (TankZombie == null)
        {
            RespawnTankZombie = true;
            TankZombie = GameObject.Find("Tank Zombie(Clone)"); 
        }
        if (SpitterZombie == null)
        {
            RespawnSpitterZombie = true;
            SpitterZombie = GameObject.Find("Spitter(Clone)");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PrintHPOfZombies();
        if (RespawnCrawlZombie == true)
        {
            SpawnZombies.CrawlSpawn = true;
            RespawnCrawlZombie = false;
        }
        else if (RespawnRegularZombie == true)
        {
            SpawnZombies.RegularSpawn = true;
            RespawnRegularZombie = false;
        }
        else if (RespawnTankZombie == true)
        {
            SpawnZombies.TankSpawn = true;
            RespawnTankZombie = false;
        }
        else if (RespawnSpitterZombie == true)
        {
            SpawnZombies.SpitterSpawn = true;
            RespawnSpitterZombie = false;
        }
    }
}
