using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Rooms
{
    public class RoomManager : Singleton<RoomManager>
    {
        private static readonly Quaternion HalfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        
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
        
        
        public void Teleport(Transform inTransform, Transform subject, Transform outTransform, Pathway outPath)
        {
            Transform subjectTransform = subject.transform;

            // Update position of object.
            Vector3 relativePos = inTransform.InverseTransformPoint(subjectTransform.position);
            relativePos = HalfTurn * relativePos;
            subjectTransform.position = outTransform.TransformPoint(relativePos);

            // Update rotation of object.
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * subjectTransform.rotation;
            relativeRot = HalfTurn * relativeRot;
            subjectTransform.rotation = outTransform.rotation * relativeRot;

            Physics.SyncTransforms();

            // Update velocity of rigidbody.
            if (subject.gameObject.TryGetComponent(out Rigidbody rb))
            {
                Vector3 relativeVel = inTransform.InverseTransformDirection(rb.velocity);
                relativeVel = HalfTurn * relativeVel;
                rb.velocity = outTransform.TransformDirection(relativeVel);
            }

            _currentRoom = GetRoom(outPath.Parent);
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