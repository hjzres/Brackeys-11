using UnityEngine;
using Input = Utils.Input;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Walk")]
        [SerializeField] private float walkSpeed = 1.5f;
        [SerializeField] private float walkFov = 1f;

        [Header("Run")]
        [SerializeField] private float runSpeed = 3f;
        [SerializeField] private float runFov = 1.5f;
        
        [Header("Jump")]
        [SerializeField] private float jumpHeight = 3f;

        [Header("Gravity")]
        [SerializeField] private float gravity = -10f;
        
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
            if (Input.Jump() && _controller.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        void DetermineHorizontalVelocity() {
            float x = Input.XMovement();
            float z = Input.ZMovement();   
        
            Vector3 velH = transform.right * x + transform.forward * z;
            if (velH.magnitude > 1)
            {
                velH.Normalize();
            }

            float speed = walkSpeed;
            _lookController.FovMultiplier = walkFov;
            if (Input.Sprint()) {
                speed = runSpeed;
                _lookController.FovMultiplier = runFov;
            }

            velH *= speed;

            _velocity.x = velH.x;
            _velocity.z = velH.z;
        }
    }
}
