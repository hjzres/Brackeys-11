using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Pathway[] pathways;
        [SerializeField] private RoomID identifier;
        [SerializeField] private bool starting;

        private Dictionary<PathwayID, Pathway> _pathwaysId;

        public bool Starting => starting;
        public RoomID Identifier => identifier;
        public bool GoEnabled
        {
            set => gameObject.SetActive(value);
            get => gameObject.activeSelf;
        }

        private void Start()
        {
            _pathwaysId = new Dictionary<PathwayID, Pathway>();
            
            foreach (var pathway in pathways)
            {
                pathway.Parent = identifier;
                foreach (var id in pathway.ValidIdentifiers)
                {
                    _pathwaysId.Add(id, pathway);
                }
            }
        }

        public Pathway GetPathwayWithId(PathwayID pathwayID)
        {
            return _pathwaysId.TryGetValue(pathwayID, out Pathway v) ? v : null;
        }
    }
}