using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)] public int resolution = 64;

    public bool autoUpdate = false;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back};
    public FaceRenderMask faceRenderMask;

    [HideInInspector] public bool shapeSettingsFoldout;
    [HideInInspector] public bool colorSettingsFoldout;

    [SerializeField, HideInInspector] MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();

    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("Planet Side");

                meshObj.transform.parent = transform;
                meshObj.AddComponent<MeshRenderer>();

                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);

            if (meshFilters[i].GetComponent<MeshCollider>() == null)
            {
                meshFilters[i].AddComponent<MeshCollider>();
            }

            meshFilters[i].GetComponent<MeshCollider>().sharedMesh = meshFilters[i].sharedMesh;

            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;

            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }

        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColors()
    {
        colorGenerator.UpdateColors();

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].UpdateUVs(colorGenerator);
            }
        }
    }
}

