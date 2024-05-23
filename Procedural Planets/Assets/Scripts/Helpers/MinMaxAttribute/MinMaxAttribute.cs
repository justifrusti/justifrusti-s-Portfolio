using UnityEngine;

public class MinMaxAttribute : PropertyAttribute
{
    public float min;
    public float max;

    public bool hasMinMax;

    public MinMaxAttribute() { }

    public MinMaxAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;

        hasMinMax = true;
    }
}
