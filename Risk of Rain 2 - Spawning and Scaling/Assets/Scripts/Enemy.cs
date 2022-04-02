using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum DirectorEnemy { CombatDirector, SceneDirector}
    public DirectorEnemy directorEnemy;

    public DifficulityScalingManager scalingManager;

    public GameObject enemyToSpawn;

    public float enemyLvl;
    public float enemyXPReward;
    public float enemyGoldReward;
    public float monsterValue;
    public float rewardMultiplier;

    public float weight;
    public int creditCost;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (directorEnemy)
        {
            case DirectorEnemy.CombatDirector:
                enemyLvl = (1 + (scalingManager.difficulityCoeff - scalingManager.playerFactor) / 0.33f);

                enemyXPReward = (scalingManager.difficulityCoeff * monsterValue * rewardMultiplier);
                enemyGoldReward = (2 * scalingManager.difficulityCoeff * monsterValue * rewardMultiplier);
                break;

            case DirectorEnemy.SceneDirector:
                enemyLvl = (1 + (scalingManager.difficulityCoeff - scalingManager.playerFactor) / 0.33f);

                enemyXPReward = (scalingManager.difficulityCoeff * monsterValue * rewardMultiplier);
                enemyGoldReward = (2 * scalingManager.difficulityCoeff * monsterValue * rewardMultiplier);
                break;
        }
    }
}
