using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InteractIndicator : MonoBehaviour
    {
        private Image _image;

        [SerializeField] private Sprite normal;
        [SerializeField] private Sprite hovered;

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
            if (obj is null)
            {
                _image.sprite = normal;
            }
            else
            {
                _image.sprite = hovered;
            }
        }
    }
}