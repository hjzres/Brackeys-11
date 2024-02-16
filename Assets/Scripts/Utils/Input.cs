using UnityEngine;

namespace Utils
{
    public enum InputContext
    {
        Gameplay,
        Dialog
    }
    
    public static class Input
    {
        public static InputContext Context = InputContext.Gameplay;
        
        public static bool Interact()
        {
            return Gameplay(UnityEngine.Input.GetMouseButtonDown(0));
        }

        public static bool Jump()
        {
            return Gameplay(UnityEngine.Input.GetButtonDown("Jump"));
        }

        public static float XMovement()
        {
            return Gameplay(UnityEngine.Input.GetAxis("Horizontal"));
        }
        
        public static float ZMovement()
        {
            return Gameplay(UnityEngine.Input.GetAxis("Vertical"));
        }

        public static bool Sprint()
        {
            return Gameplay(UnityEngine.Input.GetKey(KeyCode.LeftShift));
        }
        
        
        
        public static bool AdvanceDialog()
        {
            return Dialog(UnityEngine.Input.GetMouseButtonDown(0));
        }

        
        
        
        
        
        private static T Gameplay<T>(T val, T def = default)
        {
            return Context == InputContext.Gameplay ? val : def;
        }
        
        private static T Dialog<T>(T val, T def = default)
        {
            return Context == InputContext.Dialog ? val : def;
        }
    }
}
