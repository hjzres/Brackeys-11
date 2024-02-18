using UnityEngine;
using Interaction;

namespace Puzzles
{
    public class DirectionButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool _val;
        [SerializeField] private DirectionShiftRegister _register;

        public void Interact() {
            _register.Shift(_val);
        }
    }
}
