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
            if (startWaves && spawners.Length > 0 && !spawners[currentWave - 1].isSpawning)
            {
                isSpawning = true;
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
        yield return new WaitForSeconds(4f);
        isSpawning = false;
        StartWave();
    }
}
