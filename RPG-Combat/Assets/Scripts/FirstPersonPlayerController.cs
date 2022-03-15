using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerController : MonoBehaviour
{
    public CharacterController characterController;

    public float speed;
    public float normalSpeed;
    public float runSpeed;
    public float minX = -60f;
    public float maxX = 60f;
    public float sensitivity;
    public float cameraPitch = 0.0f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public float jumpHeight;

    public bool isGrounded;

    Vector3 velocity;

    public Transform cam;
    public Transform groundCheck;

    public LayerMask groundMask;

    public ScriptableObject characterBattleData;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        characterController.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetButtonDown("Sprint"))
        {
            speed = runSpeed;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed = normalSpeed;
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        Vector3 rotateBody = new Vector3();
        Vector3 rotateCam = new Vector3();

        float mouseX = new float();
        float mouseY = new float();

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotateCam.x = mouseY;
        rotateBody.y = mouseX;

        transform.Rotate(rotateBody * Time.deltaTime * sensitivity);

        cameraPitch -= mouseY * Time.deltaTime * sensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        cam.localEulerAngles = Vector3.right * cameraPitch;
    }
}
