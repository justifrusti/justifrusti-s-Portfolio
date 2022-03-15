using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "UnitData/NewUnit", order = 1)]
public class UnitAttacksClass : BaseUnitClass
{
    public GameObject currentUnit;

    public void Charge()
    {
        unitChargePercent = Random.Range(4.5f, 12.5f);

        if (unitChargePercent > maxUnitChargePercent)
        {
            unitChargePercent = maxUnitChargePercent;
        }
    }
}
