using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int mapIndex;
    public int ammountOfMonstersAllowed;

    [Header("Debug")]
    [Range(1.0f, 100.0f)]
    public float gameSpeed;

    [Header("Scripts")]
    public SceneDirector sceneDirector;
    public DifficulityScalingManager difficulityScalingManager;

    [Header("Lists")]
    public List<CombatDirector> combatDirectors;
    public List<Enemy> spawnableEnemies;
    public List<Transform> spawnPoints;
    public List<GameObject> enemiesAlive;

    [Header("Enemy List per Map")]
    public List<Enemy> enemiesDistantRoost;
    public List<Enemy> enemiesTitanicPlains;
    public List<Enemy> enemiesWetlandAspect;
    public List<Enemy> enemiesAbandonedAqueduct;
    public List<Enemy> enemiesRallypointDelta;
    public List<Enemy> enemiesScorchedAcres;
    public List<Enemy> enemiesAbyssalDepths;
    public List<Enemy> enemiesSirensCall;
    public List<Enemy> enemiesSkyMeadow;
    public List<Enemy> enemiesGildedCoast;

    public static int initialEnemyCount;
    public int testInitialEnemyCount;

    private void Start()
    {
        gameSpeed = 1.0f;

        AssignMonstersToList();
    }

    private void Update()
    {
        Time.timeScale = gameSpeed;

        DebugEnemyCount();
        CalculateEnemyCount();
    }

    public void DebugEnemyCount()
    {
        if (testInitialEnemyCount != initialEnemyCount)
        {
            testInitialEnemyCount = initialEnemyCount;
        }
    }

    public void CalculateEnemyCount()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!enemiesAlive.Contains(enemy))
            {
                enemiesAlive.Add(enemy);
            }
        }

        for (int i = 0; i < enemiesAlive.Count; i++)
        {
            if (enemiesAlive[i] == null)
            {
                enemiesAlive.RemoveAt(i);
                initialEnemyCount--;
            }
        }
    }

    public void AssignMonstersToList()
    {
        switch (mapIndex)
        {
            case 1:
                for (int i = 0; i < enemiesDistantRoost.Count; i++)
                {
                    if (!spawnableEnemies.Contains(enemiesDistantRoost[i]))
                    {
                        spawnableEnemies.Add(enemiesDistantRoost[i]);
                    }
                }
                break;

            case 2:
                for (int i = 0; i < enemiesTitanicPlains.Count; i++)
                {
                    if (!spawnableEnemies.Contains(enemiesTitanicPlains[i]))
                    {
                        spawnableEnemies.Add(enemiesTitanicPlains[i]);
                    }
                }
                break;

            case 3:
                for (int i = 0; i < enemiesWetlandAspect.Count; i++)
                {
                    if (!spawnableEnemies.Contains(enemiesWetlandAspect[i]))
                    {
                        spawnableEnemies.Add(enemiesWetlandAspect[i]);
                    }
                }
                break;

            case 4:
                for (int i = 0; i < enemiesAbandonedAqueduct.Count; i++)
                {
                    if (!spawnableEnemies.Contains(enemiesAbandonedAqueduct[i]))
                    {
                        spawnableEnemies.Add(enemiesAbandonedAqueduct[i]);
                    }
                }
                break;

            case 5:
                for (int i = 0; i < enemiesRallypointDelta.Count; i++)
                {
                    if (!spawnableEnemies.Contains(enemiesRallypointDelta[i]))
                    {
                        spawnableEnemies.Add(enemiesRallypointDelta[i]);
                    }
                }
                break;

            case 6:
                for (int i = 0; i < enemiesScorchedAcres.Count; i++)
                {
                    if (!spawnableEnemies.Contains(enemiesScorchedAcres[i]))
                    {
                        spawnableEnemies.Add(enemiesScorchedAcres[i]);
                    }
                }
                break;

            case 7:
                for (int i = 0; i < enemiesAbyssalDepths.Count; i++)
                {
                    if(!spawnableEnemies.Contains(enemiesAbyssalDepths[i]))
                    {
                        spawnableEnemies.Add(enemiesAbyssalDepths[i]);
                    }
                }
                break;

            case 8:
                for (int i = 0; i < enemiesSirensCall.Count; i++)
                {
                    if (!spawnableEnemies.Contains(enemiesSirensCall[i]))
                    {
                        spawnableEnemies.Add(enemiesSirensCall[i]);
                    }
                }
                break;

            case 9:
                for (int i = 0; i < enemiesSkyMeadow.Count; i++)
                {
                    if (!spawnableEnemies.Contains(enemiesSkyMeadow[i]))
                    {
                        spawnableEnemies.Add(enemiesSkyMeadow[i]);
                    }
                }
                break;

            case 10:
                for (int i = 0; i < enemiesGildedCoast.Count; i++)
                {
                    if (!spawnableEnemies.Contains(enemiesGildedCoast[i]))
                    {
                        spawnableEnemies.Add(enemiesGildedCoast[i]);
                    }
                }
                break;
        }

        for (int i = 0; i < combatDirectors.Count; i++)
        {
            combatDirectors[i].ReassignEnemies();
        }
    }
}
