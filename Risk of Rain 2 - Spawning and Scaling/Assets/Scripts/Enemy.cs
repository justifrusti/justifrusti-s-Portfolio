using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum MonsterTier { Tier1, Tier2, Tier3 }
    public MonsterTier monsterTier;

    public enum DirectorMonster { CombatDirector, SceneDirector}
    public DirectorMonster directorMonster;

    public enum MonsterCategory { BasicMonsters, MiniBosses, Champions}
    public MonsterCategory monsterCategory;

    public DifficulityScalingManager scalingManager;

    public GameObject monsterToSpawn;

    public float monsterHP;
    public float monsterDMG;
    public float monsterSpeed;
    public float regenSpeedPerSecond;
    public float armor;
    public float monsterLVL;
    public float monsterXPReward;
    public float monsterGoldReward;
    public float monsterValue;
    public float rewardMultiplier;

    [Range(0, 3)]
    public float weight;
    public int creditCost;

    // Start is called before the first frame update
    void Start()
    {
        scalingManager = GameObject.Find("DifficulityManager").GetComponent<DifficulityScalingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (directorMonster)
        {
            case DirectorMonster.CombatDirector:
                monsterLVL = (1 + (scalingManager.difficulityCoeff - scalingManager.playerFactor) / 0.33f);

                monsterXPReward = (scalingManager.difficulityCoeff * monsterValue * rewardMultiplier);
                monsterGoldReward = (2 * scalingManager.difficulityCoeff * monsterValue * rewardMultiplier);
                break;

            case DirectorMonster.SceneDirector:
                monsterLVL = (1 + (scalingManager.difficulityCoeff - scalingManager.playerFactor) / 0.33f);

                monsterXPReward = (scalingManager.difficulityCoeff * monsterValue * rewardMultiplier);
                monsterGoldReward = (2 * scalingManager.difficulityCoeff * monsterValue * rewardMultiplier);
                break;
        }
    }
}
