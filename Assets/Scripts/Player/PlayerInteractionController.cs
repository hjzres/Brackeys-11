using System;
using Interaction;
using UnityEngine;

namespace Player
{
    public class PlayerInteractionController : MonoBehaviour
    {
        public float maxDistance = 3f;
        public LayerMask interactableLayer;
        public static event Action<GameObject> OnHoverInteractable;

        private Camera _cam;
        private GameObject _lastObject;
        private bool _lastObjectDestroyed;

        private void Start()
        {
            _cam = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            GameObject obj;
            bool rc = Physics.Raycast(
                _cam.transform.position,
                _cam.transform.forward,
                out var hit, maxDistance,
                interactableLayer);


            if (rc)
            {
                obj = hit.transform.gameObject;

                if (Input.GetMouseButtonDown(0))
                {
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    var inter = obj.GetComponent<IInteractable>();
                    inter.Interact();
                }
            }
            else
            {
                obj = null;
            }

            if (obj != _lastObject || obj is null && _lastObject is not null)
            {
                _lastObject = obj;
                OnHoverInteractable?.Invoke(obj);
            }

            Debug.DrawRay(_cam.transform.position, _cam.transform.forward * maxDistance, Color.green);
        }
    }
}