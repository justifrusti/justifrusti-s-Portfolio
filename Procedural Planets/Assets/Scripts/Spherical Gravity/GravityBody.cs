using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public GravityAttractor planet;
    public Rigidbody rb;

    public bool isFlying;

    private void Awake()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            FirstPersonController controller = GetComponent<FirstPersonController>();

            if (isFlying)
            {
                isFlying = false;

                controller.walkSpeed = controller.originalWalkSpeed;
                controller.clamp = true;
            }
            else
            {
                isFlying = true;
                planet = null;

                controller.walkSpeed = 50;
                controller.clamp = false;
                
            }
        }
    }

    private void FixedUpdate()
    {
        if(planet != null && !isFlying)
        {
            planet.Attract(rb);
        }
    }
}
