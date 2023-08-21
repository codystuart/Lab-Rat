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
    bool isSpawning;

    void Start()
    {
        StartWave();
    }

    void Update()
    {
        if (currentWave <= numberOfWaves && !isSpawning)
        {
            Debug.Log("currentWave is less than max waves");
            if (startWaves && spawners.Length > 0 && !spawners[currentWave - 1].isSpawning)
            {
                isSpawning = true;
                Debug.Log("calling spawner Spawn Coroutine");
                StartCoroutine(spawners[currentWave - 1].Spawn());
                StartCoroutine(WaveTimer());

            }
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
        isSpawning = false;
        StartWave();
    }
}
