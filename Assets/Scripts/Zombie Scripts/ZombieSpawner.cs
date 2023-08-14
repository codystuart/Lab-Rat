using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] GameObject ZombieType;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] float timeBetweenSpawns = 2f;
    [SerializeField] int maxObjects;
    
    public int currentObjectsSpawnedNum;
    public bool isSpawning;

    public IEnumerator Spawn()
    {
        if (currentObjectsSpawnedNum < maxObjects)
        {
            isSpawning = true;
            for (int i = 0; i < maxObjects; i++)
            {
                currentObjectsSpawnedNum++;
                Instantiate(ZombieType, spawnPos[Random.Range(0, spawnPos.Length)].position, transform.rotation);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
            isSpawning = false;
        }
    }
}
