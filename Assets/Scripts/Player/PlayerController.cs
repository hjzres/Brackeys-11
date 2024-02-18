using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input = Utils.Input;
using static StaticData;

namespace Player
{
    [System.Serializable]
    public struct GroundTypeAudio
    {
        public PhysicMaterial physicMaterial;
        public AudioClip clip;
    }


    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask groundMask;

        [Header("Walk")] [SerializeField] private float walkSpeed = 1.5f;
        [SerializeField] private float walkFov = 1f;

        [Header("Run")] [SerializeField] private float runSpeed = 3f;
        [SerializeField] private float runFov = 1.5f;

        [Header("Jump")] [SerializeField] private float jumpHeight = 3f;

        [Header("Gravity")] [SerializeField] private float gravity = -10f;

        [Header("Audio")] [SerializeField] private AudioClip defaultStepAudio;
        [SerializeField] private GroundTypeAudio[] groundTypeAudio;

        private Vector3 _velocity;
        private CharacterController _controller;
        private PlayerLookController _lookController;

        private AudioSource _footstepAudioSource;
        private Coroutine _walkCoro;

        // Start is called before the first frame update
        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _lookController = GetComponent<PlayerLookController>();
            _footstepAudioSource = transform.Find("Feet").GetComponent<AudioSource>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            runFov = FOV + runFov - runFov;
            walkFov = FOV;
            
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

        void DetermineHorizontalVelocity()
        {
            float x = Input.XMovement();
            float z = Input.ZMovement();

            Vector3 velH = transform.right * x + transform.forward * z;
            if (velH.magnitude > 1)
            {
                velH.Normalize();
            }

            if (velH.magnitude >= 0.01 && _walkCoro is null && _controller.isGrounded)
            {
                _walkCoro = StartCoroutine(LoopWalkSound());
            }

            if ((velH.magnitude < 0.01 || !_controller.isGrounded) && _walkCoro is not null)
            {
                StopCoroutine(_walkCoro);
                _walkCoro = null;
            }

            float speed = walkSpeed;
            _lookController.FovMultiplier = walkFov;
            if (Input.Sprint())
            {
                speed = runSpeed;
                _lookController.FovMultiplier = runFov;
            }

            velH *= speed;

            _velocity.x = velH.x;
            _velocity.z = velH.z;
        }

        private IEnumerator LoopWalkSound()
        {
            var waitTime = new WaitForSeconds(0.5f);
            while (true)
            {
                WalkSound();
                yield return waitTime;
            }
        }

        private void WalkSound()
        {
            var rc = Physics.Raycast(
                _footstepAudioSource.transform.position,
                Vector3.down,
                out var hit, 0.1f,
                groundMask
            );
            if (!rc) return;
            
            
            var clip = defaultStepAudio;
            foreach (var gt in groundTypeAudio)
            {
                if (gt.physicMaterial == hit.collider.sharedMaterial)
                {
                    clip = gt.clip;
                    break;
                }
            }

            _footstepAudioSource.pitch = Random.Range(0.8f, 1.2f);
            _footstepAudioSource.PlayOneShot(clip);
        }
    }
}