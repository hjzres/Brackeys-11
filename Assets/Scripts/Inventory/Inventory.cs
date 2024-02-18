using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        public static event Action<Dictionary<ItemType, int>> OnInvChanged; 
        
        private Dictionary<ItemType, int> _items;

        private void Start()
        {
            _items = new Dictionary<ItemType, int>();
        }

        public void AddItem(ItemType itemType, int count)
        {
            if (_items.ContainsKey(itemType))
            {
                _items[itemType] += count;
                OnInvChanged?.Invoke(_items);
                Notification.ShowMessage("Obtained Item: " + itemType.itemName);
                return;
            }
            
            _items.Add(itemType, count);
            OnInvChanged?.Invoke(_items);
            Notification.ShowMessage("Obtained Item: " + itemType.itemName);
        }

        public bool RemoveItem(ItemType itemType, int count)
        {
            if (!_items.ContainsKey(itemType)) return false;
            if (_items[itemType] < 1) return false;
            
            _items[itemType] -= count;
            if (_items[itemType] <= 0)
            {
                _items.Remove(itemType);
            }
            
            OnInvChanged?.Invoke(_items);
            return true;
        }
    }
}