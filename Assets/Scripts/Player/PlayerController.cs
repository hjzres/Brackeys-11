using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public float walkFov = 1f;

    public float runSpeed = 3f;
    public float runFov = 1.5f;

    public float jumpHeight = 3f;


    public float gravity = -10f;

    private Vector3 _velocity;

    private CharacterController _controller;

    private PlayerLookController _lookController;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _lookController = GetComponent<PlayerLookController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        DetermineHorizontalVelocity();

        HandleJump();

        _velocity.y += gravity * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);

    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void DetermineHorizontalVelocity() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");   
        
        Vector3 velH = transform.right * x + transform.forward * z;
        if (velH.magnitude > 1)
        {
            velH.Normalize();
        }

        float speed = walkSpeed;
        _lookController.FovMultiplier = walkFov;
        if (Input.GetKey(KeyCode.LeftShift)) {
            speed = runSpeed;
            _lookController.FovMultiplier = runFov;
        }

        velH *= speed;

        _velocity.x = velH.x;
        _velocity.z = velH.z;
    }
}
