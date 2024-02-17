using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        public static event Action<Dictionary<ItemType, int>> OnInvChanged; 
        public static event Action<ItemType, int> OnItemObtained; 
        
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
                OnItemObtained?.Invoke(itemType, count);
                return;
            }
            
            _items.Add(itemType, count);
            OnInvChanged?.Invoke(_items);
            OnItemObtained?.Invoke(itemType, count);
        }

        public void RemoveItem(ItemType itemType, int count)
        {
            if (!_items.ContainsKey(itemType)) return;
            _items[itemType] -= count;
            if (_items[itemType] <= 0) _items.Remove(itemType);
            OnInvChanged?.Invoke(_items);
        }
    }
}