using System.Collections;
using DG.Tweening;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemObtainedNotifier : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private CanvasGroup _canvasGroup;
        
        private void Start()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        private void OnEnable()
        {
            Inventory.Inventory.OnItemObtained += PopUp;
        }

        private void OnDisable()
        {
            Inventory.Inventory.OnItemObtained -= PopUp;
        }

        private void PopUp(ItemType itemType, int count)
        {
            _text.text = "Item Obtained: " + itemType.itemName;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
            _canvasGroup.DOFade(1, 0.5f)
                .SetEase(Ease.InOutCubic);
            _canvasGroup.DOFade(0, 0.5f)
                .SetEase(Ease.InOutCubic)
                .SetDelay(3.5f);
        }
    }
}