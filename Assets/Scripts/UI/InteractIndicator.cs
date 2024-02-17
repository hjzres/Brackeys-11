using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InteractIndicator : MonoBehaviour
    {
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
            UpdateCrosshairs(null);
        }

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
            _image.enabled = !(obj is null);
        }
    }
}