using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDirector : MonoBehaviour
{
    public enum Map { DistantRoost, TitanicPlains, WetlandAspect, AbandonedAqueduct, RallypointDelta, ScorchedAcres, AbyssalDepths, SirensCall, SkyMeadow, GildedCoast }
    public Map chosenMap;

    public enum DirectorState { SpawningInteractables, SpawningEnemies, ActivatingCombatDirectors}
    public DirectorState directorState;

    public DifficulityScalingManager scalingManager;

    public List<Enemy> spawnableEnemies;
    public Enemy[] originalSpawnableEnemies;

    public List<Interactable> spawnableInteractables;
    public Interactable[] originalSpawnableInteractables;

    public Transform[] spawnPoints, interactableSpawnPoints;

    public float interactableCredit;
    public float enemyCredit;

    public int initialEnemyCount, spawnedInteractables;

    public bool vaultADOpened, vaultDROpened;

    [SerializeField]
    private float[] enemyWeights, interactableWeights;

    public List<CombatDirector> combatDirector;

    private void Awake()
    {
        foreach (Enemy enemy in originalSpawnableEnemies)
        {
            if (!spawnableEnemies.Contains(enemy))
            {
                spawnableEnemies.Add(enemy);
            }
        }

        foreach (Interactable interactable in originalSpawnableInteractables)
        {
            if (!spawnableInteractables.Contains(interactable))
            {
                spawnableInteractables.Add(interactable);
            }
        }

        enemyWeights = new float[spawnableEnemies.Count];
        interactableWeights = new float[spawnableInteractables.Count];
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < originalSpawnableEnemies.Length; i++)
        {
            if (!spawnableEnemies.Contains(originalSpawnableEnemies[i]))
            {
                spawnableEnemies.Add(originalSpawnableEnemies[i]);
            }
        }

        for (int i = 0; i < originalSpawnableInteractables.Length; i++)
        {
            if (!spawnableInteractables.Contains(originalSpawnableInteractables[i]))
            {
                spawnableInteractables.Add(originalSpawnableInteractables[i]);
            }
        }

        CalculateStartCredits();
    }

    // Update is called once per frame
    void Update()
    {
        switch(directorState)
        {
            case DirectorState.SpawningInteractables:
                if(interactableCredit != 0)
                {
                    SpawnWeightedInteractable(); 
                    ResetInteractableSpawnWeights();
                }
                else if(interactableCredit <= 0)
                {
                    directorState = DirectorState.SpawningEnemies;
                }
                break;

            case DirectorState.SpawningEnemies:
                if (initialEnemyCount != 40 || enemyCredit != 0)
                {
                    SpawnWeightedMonster();
                    ResetMonsterSpawnWeights();
                }
                else if (initialEnemyCount == 40 || enemyCredit <= 0)
                {
                    enemyCredit = 0;
                }

                if(enemyCredit == 0)
                {
                    directorState = DirectorState.ActivatingCombatDirectors;
                }
                break;

            case DirectorState.ActivatingCombatDirectors:
                for (int i = 0; i < combatDirector.Count; i++)
                {
                    combatDirector[i].enabled = true;
                }
                break;
        }

        for (int i = 0; i < spawnableEnemies.Count; i++)
        {
            if (spawnableEnemies[i].creditCost > enemyCredit)
            {
                spawnableEnemies.Remove(spawnableEnemies[i]);
            }
        }

        for (int c = 0; c < spawnableInteractables.Count; c++)
        {
            if(spawnableInteractables[c].creditCost > interactableCredit)
            {
                spawnableInteractables.Remove(spawnableInteractables[c]);
            }
        }
    }
    
    public void SpawnMonster()
    {
        if (spawnableEnemies.Count != 0)
        {
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Enemy enemyToSpawn = spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
            Transform spawnEnemy = enemyToSpawn.transform;

            Instantiate(spawnEnemy, sp.position, sp.rotation);

            print("Spawning " + enemyToSpawn);

            enemyCredit -= enemyToSpawn.creditCost;

            initialEnemyCount++;
        }else
        {
            enemyCredit = 0;
        }
    }

    public void SpawnInteractible()
    {
        if(spawnableInteractables.Count != 0)
        {
            Transform sp = interactableSpawnPoints[Random.Range(0, interactableSpawnPoints.Length)];
            Interactable interactableToSpawn = spawnableInteractables[Random.Range(0, spawnableInteractables.Count)];
            Transform spawnInteractable = interactableToSpawn.transform;

            Instantiate(spawnInteractable, sp.position, sp.rotation);

            print("Spawning" + interactableToSpawn);

            interactableCredit -= interactableToSpawn.creditCost;

            spawnedInteractables++;
        }else
        {
            interactableCredit = 0;
        }
    }

    private void SpawnWeightedMonster()
    {
        float value = Random.value;

        for (int i = 0; i < enemyWeights.Length; i++)
        {
            if(value < enemyWeights[i])
            {
                SpawnMonster();
                return;
            }

            value -= enemyWeights[i];
        }
    }

    private void SpawnWeightedInteractable()
    {
        float value = Random.value;

        for (int i = 0; i < interactableWeights.Length; i++)
        {
            if (value < interactableWeights[i])
            {
                SpawnInteractible();
                return;
            }

            value -= interactableWeights[i];
        }
    }

    public void CalculateStartCredits()
    {
        switch (chosenMap)
        {
            case Map.DistantRoost:
                interactableCredit = (180 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (100 * (1.0f + (.5f * scalingManager.difficulityCoeff)));

                if(vaultDROpened)
                {
                    interactableCredit += 160;
                }
                break;

            case Map.TitanicPlains:
                interactableCredit = (220 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (100 * (1.0f + (.5f * scalingManager.difficulityCoeff)));
                break;

            case Map.WetlandAspect:
                interactableCredit = (280 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (100 * (1.0f + (.5f * scalingManager.difficulityCoeff)));
                break;

            case Map.AbandonedAqueduct:
                interactableCredit = (220 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (100 * (1.0f + (.5f * scalingManager.difficulityCoeff)));
                break;

            case Map.RallypointDelta:
                interactableCredit = (280 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (100 * (1.0f + (.5f * scalingManager.difficulityCoeff)));
                break;

            case Map.ScorchedAcres:
                interactableCredit = (280 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (100 * (1.0f + (.5f * scalingManager.difficulityCoeff)));
                break;

            case Map.AbyssalDepths:
                interactableCredit = (400 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (230 * (1.0f + (.5f * scalingManager.difficulityCoeff)));

                if (vaultADOpened)
                {
                    interactableCredit += 160;
                }
                break;

            case Map.SirensCall:
                interactableCredit = (400 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (230 * (1.0f + (.5f * scalingManager.difficulityCoeff)));
                break;

            case Map.SkyMeadow:
                interactableCredit = (520 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (300 * (1.0f + (.5f * scalingManager.difficulityCoeff)));
                break;

            case Map.GildedCoast:
                interactableCredit = (0 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (200 * (1.0f + (.5f * scalingManager.difficulityCoeff)));
                break;
        }

        enemyCredit = Mathf.RoundToInt(enemyCredit);
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

    private void ResetInteractableSpawnWeights()
    {
        float totalInteractableWeight = 0;

        for (int i = 0; i < spawnableInteractables.Count; i++)
        {
            interactableWeights[i] = spawnableInteractables[i].weight;
            totalInteractableWeight += interactableWeights[i];
        }

        for (int i = 0; i < interactableWeights.Length; i++)
        {
            interactableWeights[i] = interactableWeights[i] / totalInteractableWeight;
        }
    }
}
