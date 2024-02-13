using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Rooms
{
    public class Pathway : MonoBehaviour
    {
        private static bool _inUse;
        private static readonly Quaternion HalfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);

        [NonSerialized] public RoomID Parent;

        [Header("Logic")]
        [SerializeField] private Room destinationRoom;
        [SerializeField] private PathwayID destinationPathway;
        [SerializeField] private PathwayID[] validIds;

        [Header("Rendering")] 
        [SerializeField] private MeshRenderer screen;
        [SerializeField] private Camera portalCamera;

        private RenderTexture _renderTexture;
        private Plane _thisPortalPlane;

        public Room DestinationRoom => destinationRoom;
        public PathwayID DestinationPathway => destinationPathway;
        public PathwayID[] ValidIdentifiers => validIds;

        private void Start()
        {
            _renderTexture = CreateRenderTexture();
            _thisPortalPlane = new Plane(transform.forward, transform.position);
            screen.material.mainTexture = _renderTexture;
            portalCamera.targetTexture = _renderTexture;
            portalCamera.enabled = false;
        }

        public void Render(ScriptableRenderContext ctx, Camera mainCamera)
        {
            if (!VisibleFromCamera(mainCamera))
            {
                return;
            }

            if (_renderTexture == null || _renderTexture.width != Screen.width || _renderTexture.height != Screen.height)
            {
                if (_renderTexture != null)
                {
                    _renderTexture.Release();
                }

                _renderTexture = CreateRenderTexture();
            }

            Pathway outPath = RoomManager.Instance.GetDestinationPathway(this);

            Transform outPathTransform = outPath.transform;
            Transform mainCamTransform = mainCamera.transform;
            Vector3 mainCamPos = mainCamTransform.position;
            
            Vector3 relativePosition = transform.InverseTransformPoint(mainCamPos);
            relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
            portalCamera.transform.position = outPathTransform.TransformPoint(relativePosition);

            Vector3 relativeRot = transform.InverseTransformDirection(mainCamTransform.forward);
            relativeRot = Vector3.Scale(relativeRot, new Vector3(-1, 1, -1));
            portalCamera.transform.forward = outPathTransform.TransformDirection(relativeRot);

            // Set the camera's oblique view frustum.
            Vector3 forward = outPathTransform.forward;
            int direction = Math.Sign(Vector3.Dot((mainCamPos - transform.position).normalized, forward));
            float dstFromPortal = _thisPortalPlane.GetDistanceToPoint(mainCamPos);
            
            Plane p = new Plane(direction * forward, outPathTransform.position - forward * dstFromPortal * 0.5f);
            Vector4 clipPlane = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
            Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlane;
            portalCamera.projectionMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
            
            // Render the camera to its render target.
            outPath.screen.enabled = false;
            UniversalRenderPipeline.RenderSingleCamera(ctx, portalCamera);
            outPath.screen.enabled = true;
        }

        private RenderTexture CreateRenderTexture()
        {
            return new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        }

        private bool VisibleFromCamera(Camera camera)
        {
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(frustumPlanes, screen.bounds);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_inUse)
            {
                _inUse = true;
            }
            else
            {
                _inUse = false;
                return;
            }
            
            Pathway outPath = RoomManager.Instance.GetDestinationPathway(this);
            Transform outTransform = outPath.transform;
            Transform otherTransform = other.transform;

            // Update position of object.
            Vector3 relativePos = transform.InverseTransformPoint(otherTransform.position);
            relativePos = HalfTurn * relativePos;
            otherTransform.position = outTransform.TransformPoint(relativePos);

            // Update rotation of object.
            Quaternion relativeRot = Quaternion.Inverse(transform.rotation) * otherTransform.rotation;
            relativeRot = HalfTurn * relativeRot;
            otherTransform.rotation = outTransform.rotation * relativeRot;

            Physics.SyncTransforms();

            // Update velocity of rigidbody.
            if (other.gameObject.TryGetComponent(out Rigidbody rb))
            {
                Vector3 relativeVel = transform.InverseTransformDirection(rb.velocity);
                relativeVel = HalfTurn * relativeVel;
                rb.velocity = outTransform.TransformDirection(relativeVel);
            }
        }
    }
}