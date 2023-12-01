using ParkingJamClone.Structs;
using UnityEngine;

namespace ParkingJamClone.Components
{
    public class CameraController : Singleton<CameraController>
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        public ScreenToWorldPointData ScreenToWorldPoint(Vector3 screenPos)
        {
            ScreenToWorldPointData result = new ScreenToWorldPointData();

            Vector2 relative = new Vector2(
                screenPos.x / Screen.width - 0.5f,
                screenPos.y / Screen.height - 0.5f
            );
            var cameraTransform = _camera.transform;

            if (!_camera.orthographic)
            {
                float verticalAngle = 0.5f * Mathf.Deg2Rad * _camera.fieldOfView;
                float worldHeight = 2f * Mathf.Tan(verticalAngle);
                Vector3 worldUnits = relative * worldHeight;
                worldUnits.x *= _camera.aspect;
                worldUnits.z = 1;
                Vector3 direction = cameraTransform.rotation * worldUnits;

                result.Origin = cameraTransform.position;
                result.Direction = direction;
            }
            else
            {
                Vector3 worldUnits = relative * _camera.orthographicSize * 2f;
                worldUnits.x *= _camera.aspect;
                Vector3 origin = cameraTransform.rotation * worldUnits;
                origin += cameraTransform.position;

                result.Origin = origin;
                result.Direction = cameraTransform.forward;
            }

            return result;
        }
    }
}