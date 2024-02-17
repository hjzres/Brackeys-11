using DG.Tweening;
using Interaction;
using UnityEngine;

namespace RoomSpecific
{
    public class MovePillow : MonoBehaviour, IInteractable
    {
        private bool _moved;

        public void Interact()
        {
            _moved = !_moved;
            float newPos = _moved ? -5 : 0;
            transform.DOLocalMoveX(newPos, 0.2f)
                .SetEase(Ease.InOutCirc);
        }
    }
}