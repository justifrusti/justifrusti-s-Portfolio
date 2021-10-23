using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCheck : MonoBehaviour
{
    public RaycastHit hitGround;

    public int rayDistance;

    public Transform rayLocation;

    public GameObject currentPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (Physics.Raycast(rayLocation.position, -rayLocation.up, out hitGround, rayDistance))
        {
            if (hitGround.collider.gameObject.tag == "Floor")
            {
                currentPrefab.transform.position = hitGround.point;
                print("You and the floor have now become best friends, Be proud that you achieved something in life");
            } else if (hitGround.collider.gameObject.tag != "Floor" || hitGround.collider.gameObject.tag == "Prop")
            {
                RandomPropGenerator.currentSpawnables--;
                Destroy(currentPrefab.gameObject);
                print("destroyed gameobject");
            }
        }
        else
        {
            RandomPropGenerator.currentSpawnables--;
            Destroy(currentPrefab.gameObject);
            print("destroyed gameobject");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Prop")
        {
            RandomPropGenerator.currentSpawnables--;
            Destroy(currentPrefab.gameObject);
            print("destroyed gameobject");
        }
    }
}
