using System;
using UnityEngine;
using Interaction;

public class PlayerInteractionController : MonoBehaviour
{
    public float maxDistance = 3f;

    private Camera _cam;

    private void Start()
    {
        _cam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AttemptInteraction();
        }
    }

    private void AttemptInteraction()
    {
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, maxDistance))
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            var inter = hit.transform.gameObject.GetComponent<IInteractable>();

            inter?.Interact();
        }
        
        Debug.DrawRay(_cam.transform.position, _cam.transform.forward * maxDistance, Color.green);
    }
}