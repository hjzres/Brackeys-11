using System;
using Interaction;
using Player;
using UnityEngine;

namespace UI
{
    public class Crosshairs : MonoBehaviour
    {
        public RectTransform top;
        public RectTransform bottom;
        public RectTransform left;
        public RectTransform right;

        [Header("Spacing")]
        public float defaultDistance = 10;
        public float hoverDistance = 20;

        private void OnEnable()
        {
            PlayerInteractionController.OnHoverInteractable += UpdateCrosshairs;
        }

        private void OnDisable()
        {
            PlayerInteractionController.OnHoverInteractable += UpdateCrosshairs;
        }

        private void UpdateCrosshairs(GameObject obj)
        {
            float distance = obj is not null ? hoverDistance : defaultDistance;
            
            top.anchoredPosition = new Vector2(0, distance);
            bottom.anchoredPosition = new Vector2(0, -distance);
            left.anchoredPosition = new Vector2(-distance, 0);
            right.anchoredPosition = new Vector2(distance, 0);
        }
    }
}