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
        //Remove quotes depending on ammount of enemy's
        //public int ammountOfEnemys2;
        //public int ammountOfEnemys3;

        public float spawnRate;

        public string waveName;

        public Transform enemy1;
        //Remove quotes depending on ammount of enemy's
        //public Transform enemy2;
        //public Transform enemy3;
    }
    
    //Remove quote if you want random event enemy's
    /*
    public Transform rEEnemy1;
    public Transform rEEnemy2;
    public Transform rEEnemy3;

    public Transform nEnemy1;
    public Transform nEnemy2;
    public Transform nEnemy3;
    */

    public Wave[] waves;
    private int waveIndex = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 0f;
    private float waveCountdown = 0;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.Counting;
    
    //Remove quote if you want random enemy events
    /*
    private int randomEventValue;
    public int randomEventPercentage;
    */

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
            
            //Change "Enemy" if you want to use something different than enemy's
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
        
        //Remove quote if you want random event
        /*
        if (randomEventValue == 2)
        {
            wave.enemy1 = rEEnemy1;
            wave.enemy2 = rEEnemy2;
            wave.enemy3 = rEEnemy3;
        }
        else if (randomEventValue != 2)
        {
            wave.enemy1 = nEnemy1;
            wave.enemy2 = nEnemy2;
            wave.enemy3 = nEnemy3;
        }
        */

        for (int i = 0; i < wave.ammountOfEnemys; i++)
        {
            SpawnEnemy(wave.enemy1);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }
        
        //Remove quote if you want more than 1 enemy
        /*for(int x = 0; x < wave.ammountOfEnemys2; x++)
        {
            SpawnEnemy(wave.enemy2);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        for(int y = 0; y < wave.ammountOfEnemys3; y++)
        {
            SpawnEnemy(wave.enemy3);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }*/

        state = SpawnState.Waiting;

        yield break;
    }

    void BeginNextWave()
    {
        Debug.Log("Wave Completed");
        
        //Remove quote if you want random events
        //randomEventValue = Random.Range(0, randomEventPercentage);

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
