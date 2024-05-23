using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Planet Settings/Shape Settings", fileName = "New Planet Shape Settings")]
public class ShapeSettings : ScriptableObject
{
    public float planetRadius = 1f;

    public NoiseLayer[] noiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool enabled = true;
        public bool useFirstLayerAsMask;

        public NoiseSettings noiseSettings;
    }
}
