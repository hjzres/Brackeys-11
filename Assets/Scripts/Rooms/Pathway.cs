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
            if (!screen.isVisible)
            {
                return;
            }

            if (_renderTexture == null || (_renderTexture.width != Screen.width || _renderTexture.height != Screen.height))
            {
                if (_renderTexture != null)
                {
                    _renderTexture.Release();
                }

                _renderTexture = CreateRenderTexture();
            }
            
            Pathway outPath = RoomManager.Instance.GetDestinationPathway(this);
            Transform mainCamTransform = mainCamera.transform;

            float diffInAngle = transform.rotation.eulerAngles.y - outPath.transform.rotation.eulerAngles.y;
            Quaternion rotationOffset = Quaternion.AngleAxis(diffInAngle, Vector3.up);
            Vector3 pos = rotationOffset * (transform.position - mainCamTransform.position);
            portalCamera.transform.position = outPath.transform.position - pos;
            // portalCamera.transform.localRotation = mainCamTransform.localRotation;
            
            // // Set the camera's oblique view frustum.
            Plane p = new Plane(-outPath.transform.forward, outPath.transform.position);
            Vector4 clipPlaneWorldSpace = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
            Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlaneWorldSpace;
            
            var newMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
            portalCamera.projectionMatrix = newMatrix;

            // Render the camera to its render target.
            UniversalRenderPipeline.RenderSingleCamera(ctx, portalCamera);
        }

        private RenderTexture CreateRenderTexture()
        {
            return new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        }
    }
}