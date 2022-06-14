using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : LivingEntity
{
    float ammountRemaining = 1;
    const float consumeSpeed = 8;

    public float Consume(float ammount)
    {
        float ammountConsumed = Mathf.Max(0, Mathf.Min(ammountRemaining, ammount));
        ammountRemaining -= ammount * consumeSpeed;

        transform.localScale = Vector3.one * ammountRemaining;

        if(ammountRemaining <= 0)
        {
            Die(CauseOfDeath.Eaten);
        }

        return ammountConsumed;
    }

    public float AmmountRemaining
    {
        get
        {
            return ammountRemaining;
        }
    }
}
