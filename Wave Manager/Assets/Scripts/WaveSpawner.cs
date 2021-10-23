using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting};

    [System.Serializable]
    public class Wave
    {
        public int ammountOfEnemys;
        public int ammountOfEnemys2;
        public int ammountOfEnemys3;

        public float spawnRate;

        public string waveName;

        public Transform enemy1;
        public Transform enemy2;
        public Transform enemy3;
    }

    public Transform bMEnemy1;
    public Transform bMEnemy2;
    public Transform bMEnemy3;

    public Transform nEnemy1;
    public Transform nEnemy2;
    public Transform nEnemy3;

    public Wave[] waves;
    private int waveIndex = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 0f;
    private float waveCountdown = 0;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.Counting;

    private int bloodMoonValue;
    public int bloodMoonPercentage;

    // Start is called before the first frame update
    void Start()
    {
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("error, there are no spawnpoints available");
        }

        waveCountdown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == SpawnState.Waiting)
        {
            if(!EnemyIsAlive())
            {
                BeginNextWave();
            }else
            {
                return;
            }
        }

        if(waveCountdown <= 0)
        {
            if(state != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves[waveIndex]));
            }
        }else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;

        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning Wave: " + wave.waveName);
        state = SpawnState.Spawning;

        if (bloodMoonValue == 2)
        {
            wave.enemy1 = bMEnemy1;
            wave.enemy2 = bMEnemy2;
            wave.enemy3 = bMEnemy3;
        }
        else if (bloodMoonValue != 2)
        {
            wave.enemy1 = nEnemy1;
            wave.enemy2 = nEnemy2;
            wave.enemy3 = nEnemy3;
        }

        for (int i = 0; i < wave.ammountOfEnemys; i++)
        {
            SpawnEnemy(wave.enemy1);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        for(int x = 0; x < wave.ammountOfEnemys2; x++)
        {
            SpawnEnemy(wave.enemy2);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        for(int y = 0; y < wave.ammountOfEnemys3; y++)
        {
            SpawnEnemy(wave.enemy3);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        state = SpawnState.Waiting;

        yield break;
    }

    void BeginNextWave()
    {
        Debug.Log("Wave Completed");

        bloodMoonValue = Random.Range(0, bloodMoonPercentage);

        //Add code for increasing wave number here if needed

        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;

        if (waveIndex + 1 > waves.Length - 1)
        {
            print("Completed all waves! Insert finish screen or something");
        }
        else
        {
            waveIndex++;
        }
    }

    void SpawnEnemy(Transform enemy)
    {
        Debug.Log("Spawning Enemy: " + enemy.name);

        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Transform newEnemy = Instantiate(enemy, sp.position, sp.rotation);
    }
}
