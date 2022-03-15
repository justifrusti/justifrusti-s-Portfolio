using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUnitClass : ScriptableObject
{
    public string unitName;

    public int unitLevel;
    public int unitHealth;
    public int unitMaxHealth;
    public int unitDamage;
    public int unitCritDamage;

    public float unitCritChance;
    public float unitChargePercent;
    public float maxUnitChargePercent;

    public Sprite unitSprite;
}

public class LevelDecider : MonoBehaviour
{
    BaseUnitClass unitClass;

    private void Start()
    {
        unitClass = GetComponent<BaseUnitClass>();
    }

    private void Update()
    {
        for (int i = 0; i < 99; i++)
        {
            unitClass.unitMaxHealth *= (1 * i);
            unitClass.unitDamage *= (1 * i);
            unitClass.unitCritDamage *= (1 * i);
            unitClass.unitCritChance *= (.0075f * i);
        }
    }
}
