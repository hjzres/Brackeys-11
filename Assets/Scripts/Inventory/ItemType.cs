using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item Type", menuName = "Game/New Item Type")]
    public class ItemType : ScriptableObject
    {
        public string itemName;
        public Sprite itemIcon;
    }
}