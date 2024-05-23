using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GravityBody))]
public class FirstPersonController : MonoBehaviour
{
    public enum MouseState
    {
        Locked,
        Free
    }

    public enum CameraMode
    {
        Player,
        SolarMap
    }

    public enum MoveMode
    {
        Walk,
        Fly
    }

    public MouseState mouseState;
    public CameraMode cameraMode;

    public Camera playerCam;
    public Camera solarCam;

    public GameObject solarUI;
    
    [HideInInspector] public float walkSpeed;
    [HideInInspector] public bool clamp;

    // public vars
    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float originalWalkSpeed = 6;
    public float jumpForce = 220;
    public LayerMask groundedMask;

    // System vars
    bool grounded;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    float verticalLookRotation;
    Transform cameraTransform;
    Rigidbody rigidbody;

    public List<GravityAttractor> planets;
    public GravityBody body;


    void Awake()
    {
        walkSpeed = originalWalkSpeed;

        cameraTransform = Camera.main.transform;
        rigidbody = GetComponent<Rigidbody>();

        GameObject[] p = GameObject.FindGameObjectsWithTag("Planet");

        foreach (GameObject pl in p)
        {
            if (pl.gameObject.GetComponent<GravityAttractor>() != null)
            {
                planets.Add(pl.GetComponent<GravityAttractor>());
            }
        }

        if(cameraMode == CameraMode.SolarMap)
        {
            walkSpeed = 0;

            solarUI.SetActive(true);

            solarCam.GetComponent<Camera>().enabled = true;
            playerCam.GetComponent<Camera>().enabled = false;
        }else
        {
            walkSpeed = originalWalkSpeed;

            solarUI.SetActive(false);

            playerCam.GetComponent<Camera>().enabled = true;
            solarCam.GetComponent<Camera>().enabled = false;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(mouseState == MouseState.Locked)
            {
                mouseState = MouseState.Free;
            }else
            {
                mouseState = MouseState.Locked;
            }

            if(mouseState == MouseState.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            if(cameraMode == CameraMode.Player)
            {
                cameraMode = CameraMode.SolarMap;
            }else
            {
                cameraMode = CameraMode.Player;
            }

            if (cameraMode == CameraMode.SolarMap)
            {
                walkSpeed = 0;

                solarUI.SetActive(true);

                solarCam.GetComponent<Camera>().enabled = true;
                playerCam.GetComponent<Camera>().enabled = false;
            }
            else
            {
                walkSpeed = originalWalkSpeed;

                solarUI.SetActive(false);

                playerCam.GetComponent<Camera>().enabled = true;
                solarCam.GetComponent<Camera>().enabled = false;
            }
        }

        for (int i = 0; i < planets.Count; i++)
        {
            float dst = Vector3.Distance(transform.position, planets[i].transform.position);

            if (dst < planets[i].gravitationalField)
            {
                body.planet = planets[i];
            }
        }

        // Look rotation:
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
        
        if(clamp)
        {
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        }

        cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

        // Calculate movement:
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
        Vector3 targetMoveAmount = moveDir * walkSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(transform.up * jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Apply movement to rigidbody
        Vector3 localMove = rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rigidbody.MovePosition(localMove);
    }
}
