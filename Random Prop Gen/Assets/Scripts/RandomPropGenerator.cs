using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPropGenerator : MonoBehaviour
{
    public Transform[] spawnablePrefabs;

    public Transform spawnablePoints;

    public GameObject currentObject;

    public int radius;
    public static int currentSpawnables;
    public int maxSpawnables;

    public RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spawnablePoints.position = Random.insideUnitSphere * radius;

        if(currentSpawnables <= maxSpawnables)
        {
            PropSpawner();
        }
        else
        {
            print("Error 404: Max Spawnables reached");
            this.enabled = false;
        }  
    }

    public void PropSpawner()
    {
        Transform spawnables = spawnablePrefabs[Random.Range(0, spawnablePrefabs.Length)];
        Transform newSpawn = Instantiate(spawnables, spawnablePoints.position, spawnablePoints.rotation);
        currentSpawnables++;
        print("I am getting a depression from coding this shit so yeah tree spawned go brrrr...");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
