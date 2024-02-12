using UnityEngine;

namespace Interaction
{
    public class Delete : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Destroy(gameObject);
        }
    }
}
