using System.Collections.Generic;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private RectTransform itemsListParent;

        private void Start()
        {
            UpdateUI(new Dictionary<ItemType, int>());
        }

        private void OnEnable()
        {
            Inventory.Inventory.OnInvChanged += UpdateUI;
        }

        private void OnDisable()
        {
            Inventory.Inventory.OnInvChanged -= UpdateUI;
        }

        private void UpdateUI(Dictionary<ItemType, int> items)
        {
            DestroyAll();
            foreach (var item in items)
            {
                GameObject itemUI = Instantiate(itemPrefab, itemsListParent);
                itemUI.GetComponentInChildren<Image>().sprite = item.Key.itemIcon;
                itemUI.GetComponentInChildren<TextMeshProUGUI>().text = item.Key.itemName + " x" + item.Value;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(itemsListParent);
        }

        private void DestroyAll()
        {
            for (int i = 0; i < itemsListParent.childCount; i++)
            {
                Transform child = itemsListParent.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }
}