using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDirector : MonoBehaviour
{
    public enum ActiveState { Activated, Deactivated };
    public ActiveState activeState;

    public enum DirectorType { Slow, Fast}
    public DirectorType directorType;

    [Header("Scripts")]
    public DifficulityScalingManager difficulityScalingManager;
    public GameManager gameManager;

    [Header("Credits")]
    public float monsterCredits = 0;
    public int monsterCreditsInt;
    public float creditMultiplier;
    public float creditsPerSecond;

    public float creditsToOtherDirector;

    [Header("Combat Directors")]
    public List<CombatDirector> combatDirectors;

    [Header("Target Spawning")]
    public List<Transform> players;
    public Transform targetedPlayer;

    public float minDst, maxDst;
    public float minTime, maxTime;

    [Header("Enemy Spawning")]
    //public List<Transform> spawnPoints;
    public List<Enemy> spawnableEnemies;

    public float timeTillNextSpawn, retargetTimer;

    public bool foundValidEnemy, firstSpawn;

    public int spawnedMonstersThisWave, maxMonsters;
    
    //Remove after Debugging
    public int creditsNeededDebugInt;

    Enemy monsterToSpawn;
    Transform spawnMonster;
    GameObject spawnedMonster;

    Enemy.MonsterTier savedMonsterTier;

    [SerializeField]
    private float[] monsterWeights;

    bool triggeredTooManyMonsters = false;

    private void Awake()
    {
        gameManager = GameObject.Find("Managers").GetComponent<GameManager>();

        for (int i = 0; i < gameManager.spawnableEnemies.Count; i++)
        {
            if (!spawnableEnemies.Contains(gameManager.spawnableEnemies[i]))
            {
                spawnableEnemies.Add(gameManager.spawnableEnemies[i]);

                spawnableEnemies[i].monsterTier = Enemy.MonsterTier.Tier1;
            }
        }

        monsterWeights = new float[spawnableEnemies.Count];
    }

    // Start is called before the first frame update
    void Start()
    {
        maxMonsters = gameManager.ammountOfMonstersAllowed;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player.transform);
        }

        if(directorType == DirectorType.Slow)
        {
            minTime = 10;
            maxTime = 20;
        }else if(directorType == DirectorType.Fast)
        {
            minTime = 0;
            maxTime = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeTillNextSpawn > 0 && !foundValidEnemy)
        {
            timeTillNextSpawn -= Time.deltaTime;
        }

        if(retargetTimer > 0)
        {
            retargetTimer -= Time.deltaTime;
        }else
        {
            retargetTimer = Random.Range(minTime, maxTime + 1);

            targetedPlayer = players[Random.Range(0, players.Count)];
        }

        switch (activeState)
        {
            case ActiveState.Activated:
                GetCredits();

                if(monsterToSpawn == null)
                {
                    SpawnWeightedMonster();
                    ResetMonsterSpawnWeights();
                }

                if (monsterCreditsInt > 0 && timeTillNextSpawn <= 0)
                {
                    SpawnWeightedMonster();
                    ResetMonsterSpawnWeights();

                    timeTillNextSpawn = Random.Range(minTime, maxTime + 1);
                }
                else if (monsterCreditsInt <= 0)
                {
                    monsterCreditsInt = 0;
                }

                if(foundValidEnemy && monsterToSpawn.creditCost <= monsterCreditsInt)
                {
                    SpawnMonster();
                }else if(foundValidEnemy && monsterToSpawn.creditCost > monsterCreditsInt)
                {
                    foundValidEnemy = false;
                }
                break;

            case ActiveState.Deactivated:
                CombatDirector combatDirector = this.gameObject.GetComponent<CombatDirector>();

                creditsToOtherDirector = (monsterCreditsInt / 10) * 4;

                for (int i = 0; i < gameManager.combatDirectors.Count; i++)
                {
                    if (!combatDirectors.Contains(gameManager.combatDirectors[i]))
                    {
                        combatDirectors.Add(gameManager.combatDirectors[i]);
                    }
                }

                CombatDirector directorToGive = combatDirectors[Random.Range(0, combatDirectors.Count)];

                directorToGive.monsterCredits += creditsToOtherDirector;

                combatDirector.enabled = false;
                break;
        }
    }

    public void GetCredits()
    {
        creditsPerSecond = creditMultiplier * (1 + .4f * difficulityScalingManager.difficulityCoeff) * (difficulityScalingManager.playerCount + 1) / 2;

        monsterCredits += (creditsPerSecond * Time.deltaTime);
        monsterCredits = Mathf.Round(monsterCredits * 1000f) / 1000f;

        if(monsterCredits >= 1.000f)
        {
            monsterCreditsInt++;

            monsterCredits = 0;
        }
    }

    public void SpawnMonster()
    {
        float spOffset = Random.Range(minDst, maxDst);
        Vector3 offset = new Vector3(spOffset, 0, spOffset);

        if (GameManager.initialEnemyCount < maxMonsters)
        {
            triggeredTooManyMonsters = false;

            if (spawnableEnemies.Count != 0)
            {
                if (!foundValidEnemy)
                {
                    monsterToSpawn = spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
                    spawnMonster = monsterToSpawn.transform;

                    if (monsterToSpawn != null)
                    {
                        CheckSpawnCardValid();
                    }

                    //Remove after Debugging 
                    creditsNeededDebugInt = monsterToSpawn.creditCost;
                }
                
                if (monsterToSpawn.creditCost <= monsterCreditsInt && foundValidEnemy)
                {
                    if(spawnedMonstersThisWave == 6)
                    {
                        foundValidEnemy = false;

                        spawnedMonstersThisWave = 0;
                    }

                    Transform spawnedMonsterT = Instantiate(spawnMonster, targetedPlayer.position + offset, targetedPlayer.rotation);
                    spawnedMonster = spawnedMonsterT.gameObject;

                    CalculateMonsterTier();

                    spawnedMonstersThisWave++;

                    GameManager.initialEnemyCount++;
                }
                else if (monsterToSpawn.creditCost >= monsterCreditsInt && foundValidEnemy)
                {
                    print("Not Enough credit to spawn enemy");

                    foundValidEnemy = false;

                    spawnedMonstersThisWave = 0;
                }
            }
        }else
        {
            if(!triggeredTooManyMonsters)
            {
                print("Too Many Enemies");

                triggeredTooManyMonsters = true;
            }
        }
    }

    private void SpawnWeightedMonster()
    {
        float value = Random.value;

        for (int i = 0; i < monsterWeights.Length; i++)
        {
            if (value < monsterWeights[i])
            {
                SpawnMonster();
                return;
            }

            value -= monsterWeights[i];
        }
    }

    private void ResetMonsterSpawnWeights()
    {
        float totalEnemyWeight = 0;

        for (int i = 0; i < spawnableEnemies.Count; i++)
        {
            monsterWeights[i] = spawnableEnemies[i].weight;
            totalEnemyWeight += monsterWeights[i];
        }

        for (int i = 0; i < monsterWeights.Length; i++)
        {
            monsterWeights[i] = monsterWeights[i] / totalEnemyWeight;
        }
    }

    private void CalculateMonsterTier()
    {
        //First Spawn check breaky??
        int tier2Cost = monsterToSpawn.creditCost * 6;
        int tier3Cost = monsterToSpawn.creditCost * 36;

        if (!firstSpawn)
        {
            spawnedMonster.GetComponent<Enemy>().monsterTier = savedMonsterTier;
        }

        if (firstSpawn)
        {
            if (difficulityScalingManager.stagesCompleted <= 4)
            {
                if (monsterCreditsInt >= tier2Cost)
                {
                    spawnedMonster.GetComponent<Enemy>().monsterTier = Enemy.MonsterTier.Tier2;
                    savedMonsterTier = Enemy.MonsterTier.Tier2;
                } else
                {
                    spawnedMonster.GetComponent<Enemy>().monsterTier = Enemy.MonsterTier.Tier1;
                    savedMonsterTier = Enemy.MonsterTier.Tier1;
                }
            } else if (difficulityScalingManager.stagesCompleted > 4)
            {
                if (monsterCreditsInt >= tier3Cost)
                {
                    spawnedMonster.GetComponent<Enemy>().monsterTier = Enemy.MonsterTier.Tier3;
                    savedMonsterTier = Enemy.MonsterTier.Tier3;
                }
                else if (monsterCreditsInt >= tier2Cost)
                {
                    spawnedMonster.GetComponent<Enemy>().monsterTier = Enemy.MonsterTier.Tier2;
                    savedMonsterTier = Enemy.MonsterTier.Tier2;
                }
                else
                {
                    spawnedMonster.GetComponent<Enemy>().monsterTier = Enemy.MonsterTier.Tier1;
                    savedMonsterTier = Enemy.MonsterTier.Tier1;
                }
            }

            firstSpawn = false;
        }

        switch (monsterToSpawn.monsterTier)
        {
            case Enemy.MonsterTier.Tier1:
                spawnedMonster.GetComponent<Enemy>().creditCost = monsterToSpawn.creditCost * 1;
                spawnedMonster.GetComponent<Enemy>().monsterHP = monsterToSpawn.monsterHP * 1;
                spawnedMonster.GetComponent<Enemy>().monsterDMG = monsterToSpawn.monsterDMG * 1;
                break;

            case Enemy.MonsterTier.Tier2:
                spawnedMonster.GetComponent<Enemy>().creditCost = monsterToSpawn.creditCost * 6;
                spawnedMonster.GetComponent<Enemy>().monsterHP = monsterToSpawn.monsterHP * 4;
                spawnedMonster.GetComponent<Enemy>().monsterDMG = monsterToSpawn.monsterDMG * 2;
                break;

            case Enemy.MonsterTier.Tier3:
                spawnedMonster.GetComponent<Enemy>().creditCost = monsterToSpawn.creditCost * 36;
                spawnedMonster.GetComponent<Enemy>().monsterHP = monsterToSpawn.monsterHP * 18;
                spawnedMonster.GetComponent<Enemy>().monsterDMG = monsterToSpawn.monsterDMG * 6;
                break;
        }

        if(spawnedMonster.GetComponent<Enemy>().creditCost > monsterCreditsInt)
        {
            Destroy(spawnedMonster);
            GameManager.initialEnemyCount--;

            spawnedMonstersThisWave = 0;

            foundValidEnemy = false;
        }

        monsterCreditsInt -= spawnedMonster.GetComponent<Enemy>().creditCost;

        print("Spawning " + monsterToSpawn + " of " + spawnedMonster.GetComponent<Enemy>().monsterTier);
    }

    public void CheckSpawnCardValid()
    {
        if(spawnedMonstersThisWave != 6)
        {
            //Add if statement to check if stage is valid

            IsMonsterTooCheap();
        }
    }

    public void IsMonsterTooCheap()
    {
        int creditsForTooCheap = ((monsterToSpawn.creditCost * 36) * 6);

        if (creditsForTooCheap > monsterCreditsInt)
        {
            foundValidEnemy = true;

            firstSpawn = true;
        }
        else if(creditsForTooCheap < monsterCreditsInt)
        {
            foundValidEnemy = false;
        }
    }

    public void ReassignEnemies()
    {
        for (int i = 0; i < gameManager.spawnableEnemies.Count; i++)
        {
            if (!spawnableEnemies.Contains(gameManager.spawnableEnemies[i]))
            {
                spawnableEnemies.Add(gameManager.spawnableEnemies[i]);

                spawnableEnemies[i].monsterTier = Enemy.MonsterTier.Tier1;
            }
        }

        monsterWeights = new float[spawnableEnemies.Count];
    }
}
