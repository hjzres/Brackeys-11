using Interaction;
using Inventory;
using Player;
using UI;
using UnityEngine;

namespace RoomSpecific
{
    public class LockedDoor : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemType requiredItem;
        [SerializeField] private AutoDoor autoDoor;

        private void Start()
        {
            autoDoor.enabled = false;
        }

        public void Interact()
        {
            if (!PlayerInstance.Instance.Inventory.RemoveItem(requiredItem, 1))
            {
                Notification.ShowMessage("Use " + requiredItem.itemName + " to interact");
            }
            else
            {
                autoDoor.enabled = true;
                gameObject.layer = 0;
                Destroy(this);
            }
        }
    }
}