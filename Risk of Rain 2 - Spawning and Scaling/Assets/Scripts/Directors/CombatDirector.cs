using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDirector : MonoBehaviour
{
    public enum ActiveState { Activated, Deactivated };
    public ActiveState activeState;

    [Header("Scripts")]
    public DifficulityScalingManager difficulityScalingManager;
    public GameManager gameManager;

    [Header("Credits")]
    public float enemyCredits = 0;
    public int enemyCreditsInt;
    public float creditMultiplier;
    public float creditsPerSecond;

    public float creditsToOtherDirector;

    [Header("Combat Directors")]
    public List<CombatDirector> combatDirectors;

    [Header("Enemy Spawning")]
    public List<Transform> spawnPoints;

    public List<Enemy> spawnableEnemies;

    public int initialEnemyCount;

    public bool foundValidEnemy;

    Transform sp;
    Enemy enemyToSpawn;
    Transform spawnEnemy;

    [SerializeField]
    private float[] enemyWeights;

    private void Awake()
    {
        gameManager = GameObject.Find("Managers").GetComponent<GameManager>();

        for (int i = 0; i < gameManager.spawnableEnemies.Count; i++)
        {
            if (!spawnableEnemies.Contains(gameManager.spawnableEnemies[i]))
            {
                spawnableEnemies.Add(gameManager.spawnableEnemies[i]);
            }
        }

        for (int i = 0; i < gameManager.spawnPoints.Count; i++)
        {
            if(!spawnPoints.Contains(gameManager.spawnPoints[i]))
            {
                spawnPoints.Add(gameManager.spawnPoints[i]);
            }
        }

        enemyWeights = new float[spawnableEnemies.Count];
    }

    // Start is called before the first frame update
    void Start()
    {
        sp = spawnPoints[Random.Range(0, spawnPoints.Count)];
        enemyToSpawn = spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
        spawnEnemy = enemyToSpawn.transform;
    }

    // Update is called once per frame
    void Update()
    {
        GetCredits();

        switch(activeState)
        {
            case ActiveState.Activated:
                GetCredits();

                if (enemyCreditsInt != 0)
                {
                    SpawnWeightedMonster();
                    ResetMonsterSpawnWeights();
                }
                else if (enemyCreditsInt <= 0)
                {
                    enemyCreditsInt = 0;
                }
                break;

            case ActiveState.Deactivated:
                CombatDirector combatDirector = this.gameObject.GetComponent<CombatDirector>();

                creditsToOtherDirector = (enemyCreditsInt / 10) * 4;

                for (int i = 0; i < gameManager.combatDirectors.Count; i++)
                {
                    if (!combatDirectors.Contains(gameManager.combatDirectors[i]))
                    {
                        combatDirectors.Add(gameManager.combatDirectors[i]);
                    }
                }

                CombatDirector directorToGive = combatDirectors[Random.Range(0, combatDirectors.Count)];

                directorToGive.enemyCredits += creditsToOtherDirector;

                combatDirector.enabled = false;
                break;
        }
    }

    public void GetCredits()
    {
        creditsPerSecond = creditMultiplier * (1 + .4f * difficulityScalingManager.difficulityCoeff) * (difficulityScalingManager.playerCount + 1) / 2;

        enemyCredits += (creditsPerSecond * Time.deltaTime);

        enemyCreditsInt = Mathf.RoundToInt(enemyCredits);
    }

    public void SpawnMonster()
    {
        //Add check to check how many enemies are alive, if it reaches over 40 wait with enemy spawn unitl it is below 40 again.

        if (initialEnemyCount < 40)
        {
            if (spawnableEnemies.Count != 0)
            {
                if (!foundValidEnemy)
                {
                    sp = spawnPoints[Random.Range(0, spawnPoints.Count)];
                    enemyToSpawn = spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
                    spawnEnemy = enemyToSpawn.transform;

                    foundValidEnemy = true;
                }

                if (enemyToSpawn.creditCost < enemyCreditsInt && foundValidEnemy)
                {
                    Instantiate(spawnEnemy, sp.position, sp.rotation);

                    print("Spawning " + enemyToSpawn);

                    enemyCreditsInt -= enemyToSpawn.creditCost;
                    enemyCredits -= enemyToSpawn.creditCost;

                    initialEnemyCount++;

                    foundValidEnemy = false;
                }
                else if (enemyToSpawn.creditCost >= enemyCreditsInt && foundValidEnemy)
                {
                    print("Not Enough credit to spawn enemy");
                }
            }
        }else
        {
            print("Too Many Enemies");
        }
    }

    private void SpawnWeightedMonster()
    {
        float value = Random.value;

        for (int i = 0; i < enemyWeights.Length; i++)
        {
            if (value < enemyWeights[i])
            {
                SpawnMonster();
                return;
            }

            value -= enemyWeights[i];
        }
    }

    private void ResetMonsterSpawnWeights()
    {
        float totalEnemyWeight = 0;

        for (int i = 0; i < spawnableEnemies.Count; i++)
        {
            enemyWeights[i] = spawnableEnemies[i].weight;
            totalEnemyWeight += enemyWeights[i];
        }

        for (int i = 0; i < enemyWeights.Length; i++)
        {
            enemyWeights[i] = enemyWeights[i] / totalEnemyWeight;
        }
    }
}
