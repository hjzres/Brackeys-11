using UnityEngine;
using Utils;

namespace Player
{
    [RequireComponent(typeof(Inventory.Inventory))]
    public class PlayerInstance : Singleton<PlayerInstance>
    {
        private Inventory.Inventory _inv;

        public Inventory.Inventory Inventory => _inv;

        private void Start()
        {
            _inv = GetComponent<Inventory.Inventory>();
        }
    }
}