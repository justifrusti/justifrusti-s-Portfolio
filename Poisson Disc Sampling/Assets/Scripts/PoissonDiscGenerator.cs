using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonDiscGenerator : MonoBehaviour
{
    [Header("Poisson Disc Value's")]
    public bool displayGizmos;

    public float radius = 1;
    public float displayRadius = 1;

    public Vector2 regionSize = Vector2.one;

    public int rejectionSamples = 30;

    List<Vector2> points;

    [Header("Spawnable's")]
    public Transform objectToSpawn;

    private void Update()
    {
        if(points.Count != 0)
        {
            Vector2 spawnPoint = points[Random.Range(0, points.Count)];
            Instantiate(objectToSpawn, spawnPoint, Quaternion.identity);
        }
    }

    private void OnValidate()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
    }

    private void OnDrawGizmos()
    {
        if (displayGizmos)
        {
            Gizmos.DrawWireCube(regionSize / 2, regionSize);

            if (points != null)
            {
                foreach (Vector2 point in points)
                {
                    Gizmos.DrawSphere(point, displayRadius);
                }
            }
        }
    }
}
