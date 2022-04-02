using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficulityScalingManager : MonoBehaviour
{
    public int playerCount;
    public int difficultyValue;
    public int stagesCompleted;
    public int timeInMinutes;

    public float playerFactor;
    public float timeFactor;
    public float stageFactor;
    public float difficulityCoeff;
    public float timeInSeconds;


    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateCoeff();
    }

    public void CalculateCoeff()
    {
        timeInSeconds += (1.0f * Time.deltaTime);

        if(timeInSeconds >= 60)
        {
            timeInSeconds = 0;
            timeInMinutes++;
        }

        playerFactor = (1 + 0.3f * (playerCount - 1));
        timeFactor = (0.0506f * difficultyValue * Mathf.Pow(playerCount, .2f));
        stageFactor = Mathf.Pow(1.15f, stagesCompleted);

        difficulityCoeff = ((playerFactor + timeInMinutes * timeFactor) * stageFactor);
    }
}
