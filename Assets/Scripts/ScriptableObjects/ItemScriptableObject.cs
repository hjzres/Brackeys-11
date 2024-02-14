using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class ItemScriptableObject : ScriptableObject
    {
        [SerializeField] private Sprite itemImage;
        [SerializeField] private string itemDescription;
    }
}
