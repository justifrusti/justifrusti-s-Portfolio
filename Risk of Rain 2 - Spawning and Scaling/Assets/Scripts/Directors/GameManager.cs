using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SceneDirector sceneDirector;
    public DifficulityScalingManager difficulityScalingManager;

    public List<CombatDirector> combatDirectors;
    public List<Enemy> spawnableEnemies;
    public List<Transform> spawnPoints;
}
