using Inventory;
using Player;
using UnityEngine;

namespace Interaction
{
    public class PickUp : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemType itemType;
        
        public void Interact()
        {
            PlayerInstance.Instance.Inventory.AddItem(itemType, 1);
            Destroy(gameObject);
        }
    }
}