using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatShrineDirector : MonoBehaviour
{
    public enum ActiveState { Innactive, CalculateCredits, Active, Disabled}
    public ActiveState activeState;

    [Header("Credits Calculation")]
    public int creditsAmmountInt;

    public float creditsAmmount;
    public float rewardsMultiplier;

    [Header("Scripts")]
    public DifficulityScalingManager scalingManager;
    public GameManager gameManager;

    [Header("Target Spawning")]
    public List<Transform> players;
    public Transform targetedPlayer;

    public float minDst, maxDst;

    [Header("Enemy Spawning")]
    public List<Enemy> spawnableEnemies;

    public bool foundValidEnemy, firstSpawn;

    //Remove after Debugging
    public int creditsNeededDebugInt;

    Enemy monsterToSpawn;
    Transform spawnMonster;
    GameObject spawnedMonster;

    Enemy.MonsterTier savedMonsterTier;

    [SerializeField]
    private float[] monsterWeights;

    // Start is called before the first frame update
    void Start()
    {
        ReassignEnemies();

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(activeState)
        {
            case ActiveState.Innactive:
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

                foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (!players.Contains(player.transform))
                    {
                        players.Add(player.transform);
                    }
                }

                if (spawnableEnemies == null)
                {
                    ReassignEnemies();
                }
                break;

            case ActiveState.CalculateCredits:
                creditsAmmount = 100 * scalingManager.difficulityCoeff * rewardsMultiplier;
                creditsAmmountInt = Mathf.RoundToInt(creditsAmmount);

                targetedPlayer = players[Random.Range(0, players.Count)];

                activeState = ActiveState.Active;
                break;

            case ActiveState.Active:
                if (monsterToSpawn == null)
                {
                    SpawnWeightedMonster();
                    ResetMonsterSpawnWeights();
                }

                if(creditsAmmountInt >= monsterToSpawn.creditCost)
                {
                    SpawnWeightedMonster();
                    ResetMonsterSpawnWeights();
                }

                if(creditsAmmountInt < monsterToSpawn.creditCost)
                {
                    activeState = ActiveState.Disabled;
                }

                break;

            case ActiveState.Disabled:
                CombatShrineDirector director = this.GetComponent<CombatShrineDirector>();

                director.enabled = false;
                break;
        }
    }

    public void SpawnMonster()
    {
        float spOffset = Random.Range(minDst, maxDst);
        Vector3 offset = new Vector3(spOffset, 0, spOffset);

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

            if (monsterToSpawn.creditCost <= creditsAmmountInt && foundValidEnemy)
            {
                Transform spawnedMonsterT = Instantiate(spawnMonster, targetedPlayer.position + offset, targetedPlayer.rotation);
                spawnedMonster = spawnedMonsterT.gameObject;

                CalculateMonsterTier();

                GameManager.initialEnemyCount++;
            }
            else if (monsterToSpawn.creditCost >= creditsAmmountInt && foundValidEnemy)
            {
                print("Not Enough credit to spawn enemy");

                foundValidEnemy = false;

                activeState = ActiveState.Disabled;
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
        int tier2Cost = monsterToSpawn.creditCost * 6;
        int tier3Cost = monsterToSpawn.creditCost * 36;

        if (!firstSpawn)
        {
            spawnedMonster.GetComponent<Enemy>().monsterTier = savedMonsterTier;
        }

        if (firstSpawn)
        {
            if (scalingManager.stagesCompleted <= 4)
            {
                if (creditsAmmountInt >= tier2Cost)
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
            else if (scalingManager.stagesCompleted > 4)
            {
                if (creditsAmmountInt >= tier3Cost)
                {
                    spawnedMonster.GetComponent<Enemy>().monsterTier = Enemy.MonsterTier.Tier3;
                    savedMonsterTier = Enemy.MonsterTier.Tier3;
                }
                else if (creditsAmmountInt >= tier2Cost)
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

        if (spawnedMonster.GetComponent<Enemy>().creditCost > creditsAmmountInt)
        {
            Destroy(spawnedMonster);

            foundValidEnemy = false;
        }

        creditsAmmountInt -= spawnedMonster.GetComponent<Enemy>().creditCost;

        print("Spawning " + monsterToSpawn + " of " + spawnedMonster.GetComponent<Enemy>().monsterTier);
    }

    public void CheckSpawnCardValid()
    {
        //Add if statement to check if stage is valid

        IsMonsterTooCheap();
    }

    public void IsMonsterTooCheap()
    {
        int creditsForTooCheap = ((monsterToSpawn.creditCost * 36) * 6);

        if (creditsForTooCheap > creditsAmmountInt)
        {
            foundValidEnemy = true;

            firstSpawn = true;
        }
        else if (creditsForTooCheap < creditsAmmountInt)
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
