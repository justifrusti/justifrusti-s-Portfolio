using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDirector : MonoBehaviour
{
    public enum Map { LevelOne, LevelTwo, LevelThree, LevelFour, LevelFive, LevelSix, LevelSeven, LevelEight, LevelNine, LevelTen }
    public Map chosenMap;

    public DifficulityScalingManager scalingManager;

    public float interactableCredit;
    public float enemyCredit;

    public int initialEnemyCount;

    // Start is called before the first frame update
    void Start()
    {
        CalculateStartCredits();
    }

    // Update is called once per frame
    void Update()
    {
        if(initialEnemyCount != 40 || enemyCredit <= 0)
        {

        }else
        {
            enemyCredit = 0;
        }
    }

    public void CalculateStartCredits()
    {
        switch (chosenMap)
        {
            case Map.LevelOne:
                interactableCredit = (180 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (100 * (1.0f + (.5f * scalingManager.playerCount)));
                break;

            case Map.LevelTwo:
                interactableCredit = (220 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (100 * (1.0f + (.5f * scalingManager.playerCount)));
                break;

            case Map.LevelThree:
                interactableCredit = (280 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (100 * (1.0f + (.5f * scalingManager.playerCount)));
                break;

            case Map.LevelFour:
                interactableCredit = (220 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (100 * (1.0f + (.5f * scalingManager.playerCount)));
                break;

            case Map.LevelFive:
                interactableCredit = (280 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (100 * (1.0f + (.5f * scalingManager.playerCount)));
                break;

            case Map.LevelSix:
                interactableCredit = (280 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (100 * (1.0f + (.5f * scalingManager.playerCount)));
                break;

            case Map.LevelSeven:
                interactableCredit = (400 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (230 * (1.0f + (.5f * scalingManager.playerCount)));
                break;

            case Map.LevelEight:
                interactableCredit = (400 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (230 * (1.0f + (.5f * scalingManager.playerCount)));
                break;

            case Map.LevelNine:
                interactableCredit = (520 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (300 * (1.0f + (.5f * scalingManager.playerCount)));
                break;

            case Map.LevelTen:
                interactableCredit = (0 * (1.0f + (.5f * scalingManager.playerCount)));
                enemyCredit = (200 * (1.0f + (.5f * scalingManager.playerCount)));
                break;
        }
    }
}
