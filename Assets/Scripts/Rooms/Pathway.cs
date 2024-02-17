using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Rooms
{
    public class Pathway : MonoBehaviour
    {
        private static bool _inUse;

        [NonSerialized] public RoomID Parent;

        [Header("Logic")] [SerializeField] private Room destinationRoom;
        [SerializeField] private PathwayID destinationPathway;
        [SerializeField] private PathwayID[] validIds;

        [Header("Rendering")] [SerializeField] private MeshRenderer screen;
        [SerializeField] private Camera portalCamera;
        [SerializeField] private float obliqueClippingPlaneOffsetMultiplier = 0.5f;
        [SerializeField] private float screenThicknessMul = 1.5f;

        private RenderTexture _renderTexture;
        private List<PathwayTraveller> _trackedTravellers;

        public Room DestinationRoom => destinationRoom;
        public PathwayID DestinationPathway => destinationPathway;
        public PathwayID[] ValidIdentifiers => validIds;

        private void Start()
        {
            _renderTexture = CreateRenderTexture();
            _trackedTravellers = new List<PathwayTraveller>();

            screen.material.mainTexture = _renderTexture;
            portalCamera.targetTexture = _renderTexture;
            portalCamera.enabled = false;

            if (Math.Abs(transform.localScale.z - 1) > 0.01f)
            {
                Debug.LogWarning($"Pathway of name '{name}' has a scale that is not 1 on the z index");
            }
        }

        public void Render(ScriptableRenderContext ctx, Camera mainCamera)
        {
            if (!VisibleFromCamera(mainCamera))
            {
                return;
            }

            if (_renderTexture == null || _renderTexture.width != Screen.width ||
                _renderTexture.height != Screen.height)
            {
                if (_renderTexture != null)
                {
                    _renderTexture.Release();
                }

                _renderTexture = CreateRenderTexture();
            }

            
            Pathway outPath = RoomManager.Instance.GetDestinationPathway(this);

            if (outPath == null)
            {
                return;
            }
            
            Transform outPathTransform = outPath.transform;
            Transform mainCamTransform = mainCamera.transform;
            Vector3 mainCamPos = mainCamTransform.position;
            Plane thisPortalPlane = new Plane(transform.forward, screen.bounds.ClosestPoint(mainCamPos));

            // Set portal camera position
            Vector3 relativePosition = transform.InverseTransformPoint(mainCamPos);
            relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
            portalCamera.transform.position = outPathTransform.TransformPoint(relativePosition);

            // Set portal camera rotation
            Vector3 relativeRot = transform.InverseTransformDirection(mainCamTransform.forward);
            relativeRot = Vector3.Scale(relativeRot, new Vector3(-1, 1, -1));
            portalCamera.transform.forward = outPathTransform.TransformDirection(relativeRot);

            // Set the camera's oblique view frustum.
            Vector3 forward = outPathTransform.forward;
            int direction = Math.Sign(Vector3.Dot((mainCamPos - transform.position).normalized, transform.forward));
            float dstFromPortal = thisPortalPlane.GetDistanceToPoint(mainCamPos);
            Plane p = new Plane(direction * forward, outPathTransform.position - forward * dstFromPortal * obliqueClippingPlaneOffsetMultiplier);
            
            Vector4 clipPlane = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
            Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlane;
            portalCamera.projectionMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
            
            ProtectScreenFromClipping(mainCamera, portalCamera.transform.position);

            // Render the camera to its render target.
            outPath.screen.enabled = false;
            UniversalRenderPipeline.RenderSingleCamera(ctx, portalCamera);
            outPath.screen.enabled = true;

            ProtectScreenFromClipping(mainCamera, mainCamPos);
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
            if (!other.TryGetComponent(out PathwayTraveller traveller))
            {
                return;
            }

            OnTravellerEnterPortal(traveller);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out PathwayTraveller traveller))
            {
                return;
            }

            _trackedTravellers.Remove(traveller);
        }

        private void LateUpdate()
        {
            Pathway outPath = RoomManager.Instance.GetDestinationPathway(this);
            
            if (outPath == null)
            {
                return;
            }
            
            Transform outTransform = outPath.transform;

            for (var i = 0; i < _trackedTravellers.Count; i++)
            {
                PathwayTraveller traveller = _trackedTravellers[i];
                Transform travellerTransform = traveller.transform;

                Vector3 offsetFromPortal = travellerTransform.position - transform.position;
                int portalSide = Math.Sign(Vector3.Dot(offsetFromPortal, transform.forward));
                int portalSideOld = Math.Sign(Vector3.Dot(traveller.previousOffset, transform.forward));

                // Teleport the traveller if it has crossed from one side of the portal to the other
                if (portalSide != portalSideOld)
                {
                    RoomManager.Instance.Teleport(transform, traveller.transform, outTransform, outPath);
                    // Can't rely on OnTriggerEnter/Exit to be called next frame since it depends on when FixedUpdate runs
                    outPath.OnTravellerEnterPortal(traveller);
                    _trackedTravellers.RemoveAt(i);
                    i--;
                }
                else
                {
                    traveller.previousOffset = offsetFromPortal;
                }
            }
        }

        private void OnTravellerEnterPortal(PathwayTraveller traveller)
        {
            if (!_trackedTravellers.Contains(traveller))
            {
                traveller.previousOffset = traveller.transform.position - transform.position;
                _trackedTravellers.Add(traveller);
            }
        }
        
        // ReSharper disable Unity.InefficientPropertyAccess
        private void ProtectScreenFromClipping(Camera playerCam, Vector3 viewPoint)
        {
            float halfHeight = playerCam.nearClipPlane * Mathf.Tan(playerCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float halfWidth = halfHeight * playerCam.aspect;
            float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, playerCam.nearClipPlane).magnitude;
            float screenThickness = dstToNearClipPlaneCorner * screenThicknessMul;

            Transform screenT = screen.transform;
            bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - viewPoint) > 0;
            screenT.localScale = new Vector3(screenT.localScale.x, screenT.localScale.y, screenThickness);
            screenT.localPosition = Vector3.forward * screenThickness * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
        }
    }
}