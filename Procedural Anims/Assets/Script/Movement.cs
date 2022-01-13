using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int speed;
    public int runSpeed;
    public int walkingSpeed;
    public int camSensitivity;
    public int normalCamSensitivity;
    public int lockedCamSensitivity;
    public int timesJumped;
    public int maxJumps;

    public float cameraPitch = 0.0f;

    public bool canJump;

    public Vector3 jump;

    public Transform cam;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = walkingSpeed;
        camSensitivity = normalCamSensitivity;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3();
        Vector3 rotateBody = new Vector3();
        Vector3 rotateCam = new Vector3();

        float v = new float();
        float h = new float();

        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        move.x = h;
        move.z = v;

        transform.Translate(move * Time.deltaTime * speed);

        float mouseX = new float();
        float mouseY = new float();

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotateCam.x = mouseY;
        rotateBody.y = mouseX;

        transform.Rotate(rotateBody * Time.deltaTime * camSensitivity);

        cameraPitch -= mouseY * Time.deltaTime * camSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        cam.localEulerAngles = Vector3.right * cameraPitch;

        if (Input.GetButtonDown("Sprint"))
        {
            speed = runSpeed;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed = walkingSpeed;
        }

        if (Input.GetButtonDown("Jump") && canJump == true)
        {
            if (timesJumped != maxJumps)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0);
                rb.velocity += jump;
                timesJumped++;
            }

            if (timesJumped == maxJumps)
            {
                canJump = false;
            }
        }
    }
}
