using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] GameObject ZombieType;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] int maxObjects;
    
    int currentObjectsSpawnedNum;
    public bool isSpawning;

    public IEnumerator Spawn()
    {
        if (currentObjectsSpawnedNum < maxObjects)
        {
            isSpawning = true;
            currentObjectsSpawnedNum++;
            Instantiate(ZombieType, spawnPos[Random.Range(0, spawnPos.Length)].position, transform.rotation);
            yield return new WaitForSeconds(timeBetweenSpawns);
            isSpawning = false;
        }
    }
}
