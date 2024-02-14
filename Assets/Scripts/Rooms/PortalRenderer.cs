using UnityEngine;
using UnityEngine.Rendering;

namespace Rooms
{
    public class PortalRenderer : MonoBehaviour
    {
        private Pathway[] _pathways;

        private void Start()
        {
            _pathways = FindObjectsByType<Pathway>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += Render;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= Render;
        }

        private void Render(ScriptableRenderContext ctx, Camera mainCam)
        {
            if (mainCam.cameraType == CameraType.SceneView) return;
            
            foreach (var pathway in _pathways)
            {
                pathway.Render(ctx, mainCam);
            }
        }
    }
}