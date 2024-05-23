using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Planet Settings/Color Settings", fileName = "New Planet Color Settings")]
public class ColorSettings : ScriptableObject
{
    public Material planetMaterial;
    public BiomeColorSettings biomeColorSettings;
    public Gradient oceanColor;

    [System.Serializable]
    public class BiomeColorSettings
    {
        public Biome[] biomes;
        public NoiseSettings noise;

        public float noiseOffset;
        public float noiseStrength;
        [Range(0f, 1f)] public float blendAmount;

        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tint;
            [Range(0f, 1f)] public float startHeight;
            [Range(0f, 1f)] public float tintPercent;
        }
    }
}
    
