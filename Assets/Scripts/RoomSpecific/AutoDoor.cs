using DG.Tweening;
using Player;
using Rooms;
using UnityEngine;

namespace RoomSpecific
{
    public class AutoDoor : MonoBehaviour
    {
        [SerializeField] private MeshCollider doorWing;
        [SerializeField] private Pathway portal;
        [SerializeField] private float range;

        private bool _wasOpen;
        private bool _inProgress;

        private void Start()
        {
            portal.gameObject.SetActive(false);
        }

        private void Update()
        {
            Debug.Log(_wasOpen);
            
            Vector3 playerPos = PlayerInstance.Instance.transform.position;
            
            bool inRangeOfDoor = (playerPos - transform.position).magnitude <= range;
            Pathway outPath = RoomManager.Instance.GetDestinationPathway(portal);
            bool inRangeOfOtherPortal = outPath != null &&
                                        (playerPos - outPath.transform.position).magnitude <= range;

            if (inRangeOfDoor || inRangeOfOtherPortal)
            {
                if (_wasOpen)
                {
                    return;
                }

                _wasOpen = true;
                Open();
            }
            else if (_wasOpen)
            {
                Close();
            }
        }

        private void Open()
        {
            if (_inProgress) return;
            _inProgress = true;
            portal.gameObject.SetActive(true);
            doorWing.enabled = false;
            doorWing.transform.DOLocalRotate(new Vector3(0, 120, 0), 0.5f)
                .OnComplete(() =>
                {
                    doorWing.enabled = true;
                    _inProgress = false;
                });
        }

        private void Close()
        {
            if (_inProgress) return;
            _inProgress = true;
            doorWing.enabled = false;
            doorWing.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f)
                .OnComplete(() =>
                {
                    portal.gameObject.SetActive(false);
                    doorWing.enabled = true;
                    _inProgress = false;
                    _wasOpen = false;
                });
        }
    }
}