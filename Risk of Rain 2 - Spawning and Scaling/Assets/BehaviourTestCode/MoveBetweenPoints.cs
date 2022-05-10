using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    public Transform[] targetPoints;
    public Transform currentTarget;

    public float moveSpeed;
    public float distanceOffset;

    // Start is called before the first frame update
    void Start()
    {
        currentTarget = targetPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, currentTarget.position) < distanceOffset)
        {
            SwitchTarget();
        }
    }

    public void SwitchTarget()
    {
        if(currentTarget == targetPoints[0])
        {
            currentTarget = targetPoints[1];
        }else
        {
            currentTarget = targetPoints[0];
        }
    }
}
