using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMaterials : MonoBehaviour
{
    public Material[] material;

    public static ApplyMaterials instance;

    public static ApplyMaterials Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<ApplyMaterials>();
            }

            return instance;
        }
    }

    public void ApplyMaterial(CelestialBody body)
    {
        Material mat = Array.Find(material, planet => planet.name == body.name);

        body.GetComponent<MeshRenderer>().material = mat;
    }
}
