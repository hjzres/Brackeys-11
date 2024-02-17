using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Rooms
{
    public class RoomManager : Singleton<RoomManager>
    {
        [SerializeField] private Transform player;
        [SerializeField] private Room[] allRooms;

        private Room _currentRoom;
        private Dictionary<RoomID, Room> _rooms;
        private Dictionary<RoomID, RoomID> _swaps;

        private void Start()
        {
            _rooms = new Dictionary<RoomID, Room>();
            _swaps = new Dictionary<RoomID, RoomID>();

            foreach (var r in allRooms)
            {
                _rooms.Add(r.Identifier, r);
                if (r.Starting) continue;
                r.GoEnabled = false;
            }
        }
        
        public void Swap(RoomID original, RoomID swapped)
        {
            // todo: temp
            GetRoom(original).GoEnabled = false;
            GetRoom(swapped).GoEnabled = true;

            // if its already swapped and wants to be swapped back
            if (_swaps.ContainsKey(swapped) && _swaps[swapped] == original)
            {
                _swaps.Remove(swapped);
                return;
            }
            
            if (_swaps.ContainsKey(original) && _swaps[original] == swapped)
            {
                _swaps.Remove(original);
                return;
            }
            
            _swaps.Add(original, swapped);
        }

        public Pathway GetDestinationPathway(Pathway pathway)
        {
            if (pathway.DestinationRoom == null)
            {
                return null;
            }
            
            // resolve any swaps
            Room actualDest = GetRoom(pathway.DestinationRoom.Identifier);

            if (actualDest == null)
            {
                return null;
            }
            
            // get the destination pathway on the actual room destination
            return actualDest.GetPathwayWithId(pathway.DestinationPathway);
        }

        private Room GetRoom(RoomID roomID)
        {
            RoomID resolved = ResolveRoomId(roomID);
            if (!_rooms.ContainsKey(resolved)) return null;
            return _rooms[resolved];
        }

        private RoomID ResolveRoomId(RoomID roomID)
        {
            while (true)
            {
                if (!_swaps.ContainsKey(roomID)) return roomID;
                roomID = _swaps[roomID];
            }
        }
    }
}