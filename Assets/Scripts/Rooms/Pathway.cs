using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Rooms
{
    public class Pathway : MonoBehaviour
    {
        [NonSerialized] public RoomID Parent;

        [Header("Logic")] [SerializeField] private Room destinationRoom;
        [SerializeField] private PathwayID destinationPathway;
        [SerializeField] private PathwayID[] validIds;

        [Header("Rendering")] [SerializeField] public MeshRenderer screen;
        [SerializeField] private Camera portalCamera;

        private RenderTexture _renderTexture;

        public Room DestinationRoom => destinationRoom;
        public PathwayID DestinationPathway => destinationPathway;
        public PathwayID[] ValidIdentifiers => validIds;

        private void Start()
        {
            _renderTexture = CreateRenderTexture();
            screen.material.mainTexture = _renderTexture;
            portalCamera.targetTexture = _renderTexture;
        }

        public void Render(ScriptableRenderContext ctx, Camera mainCamera)
        {
            if (!VisibleFromCamera(mainCamera))
            {
                return;
            }

            if (_renderTexture == null ||
                (_renderTexture.width != Screen.width || _renderTexture.height != Screen.height))
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

            Vector3 relativePosition = transform.InverseTransformPoint(mainCamTransform.position);
            relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
            portalCamera.transform.position = outPathTransform.TransformPoint(relativePosition);

            Vector3 relativeRot = transform.InverseTransformDirection(mainCamTransform.forward);
            relativeRot = Vector3.Scale(relativeRot, new Vector3(-1, 1, -1));
            portalCamera.transform.forward = outPathTransform.TransformDirection(relativeRot);

            // Set the camera's oblique view frustum.
            var forward = outPathTransform.forward;
            Plane p = new Plane(forward, outPathTransform.position - forward * 0.01f);
            Vector4 clipPlane = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
            Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlane;
            portalCamera.projectionMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
            
            // Render the camera to its render target.
            UniversalRenderPipeline.RenderSingleCamera(ctx, portalCamera);
        }

        private RenderTexture CreateRenderTexture()
        {
            return new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        }
        
        private bool VisibleFromCamera(Camera camera) {
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(frustumPlanes, screen.bounds);
        }
    }
}