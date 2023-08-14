using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] ZombieSpawner[] spawners;
    [SerializeField] int numberOfWaves;
    
    public int currentWave;
    public bool startWaves;

    void Start()
    {
        StartWave();
    }
    void Update()
    {
        if(spawners[currentWave-1].isSpawning == false)
        {
            if (startWaves && currentWave < numberOfWaves)
            {
                StartCoroutine(spawners[currentWave - 1].Spawn());
            }
            StartCoroutine(WaveTimer());
        }
    }

    void StartWave()
    {
        currentWave++;
        startWaves = true;
    }

    IEnumerator WaveTimer()
    {
        Debug.Log("WaveTimer started");
        yield return new WaitForSeconds(4f);
        StartWave();
    }
}
