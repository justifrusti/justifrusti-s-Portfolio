using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public float gravity = -9.8f;
    public float gravitationalField = 1.0f;

    public GameObject player;

    public bool showGizmos;

    public void Attract(Rigidbody rb)
    {
        Vector3 gravityUp = (rb.position - transform.position).normalized;
        Vector3 localUp = rb.transform.up;

        rb.AddForce(gravityUp * gravity);
        rb.rotation = Quaternion.FromToRotation(localUp, gravityUp) * rb.rotation;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnDrawGizmosSelected()
    {
        if(showGizmos)
        {
            Gizmos.DrawWireSphere(transform.position, gravitationalField);
        }
    }
}
