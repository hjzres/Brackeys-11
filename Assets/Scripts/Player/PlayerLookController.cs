using UnityEngine;

namespace Player
{
    public class PlayerLookController : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 100f;
        [SerializeField] private float fovTransitionSpeed = 100f;
    
        public float FovMultiplier
        {
            get => _fovMultiplier;
            set
            {
                _fovMultiplier = value;
                _targetFov = _originalFov * _fovMultiplier;
            }
        }

        private Camera _cam;
        private float _xRotation = 0f;
        private float _targetFov;
        private float _originalFov;
        private float _fovMultiplier = 1;


        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _cam = GetComponentInChildren<Camera>();
            _originalFov = _cam.fieldOfView;
        }

        private void Update()
        {
            if (!Application.isFocused) return;
            
            var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _cam.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.Rotate(Vector3.up, mouseX);

            _cam.fieldOfView += Mathf.Clamp(_targetFov - _cam.fieldOfView, -fovTransitionSpeed * Time.deltaTime, fovTransitionSpeed * Time.deltaTime);
        }
    }
}