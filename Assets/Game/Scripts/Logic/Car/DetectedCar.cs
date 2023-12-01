using DG.Tweening;
using ParkingJamClone.Interfaces;
using ParkingJamClone.Manager;
using ParkingJamClone.Structs;
using UnityEngine;

namespace ParkingJamClone.Logic
{
    public class SwipeableCar : IDetectable
    {
        public Vector3 CarDirection { get; }
        public Vector3 MovementDirection;

        private readonly DetectableObjectData _currDetectableObjectData;

        private ScreenToWorldPointData _manualRay;

        public void SetRayDirection(Vector3 direction)
        {
            _manualRay.Direction = direction;
            MovementDirection = _manualRay.Direction;
        }

        public SwipeableCar(DetectableObjectData objectData)
        {
            _manualRay = new ScreenToWorldPointData();
            _currDetectableObjectData = objectData;

            var position = _currDetectableObjectData.DetectableTransform.position;

            _manualRay.Origin = position;
            Vector3 calculatedDirection =
                (Calculator.LocalPointToWorld(Vector3.forward, _currDetectableObjectData.DetectableTransform) -
                 _manualRay.Origin);

            _manualRay.Direction = calculatedDirection.normalized;
            CarDirection = _manualRay.Direction;
        }

        public void OnDetected()
        {
            //the car has been touched => check its mobility
            SendManualRaycast();
        }

        public void Move(Vector3 border, Vector3[] path, float duration, float pathDistanceDelay, AnimationCurve curve)
        {
            Vector3 position = _currDetectableObjectData.DetectableTransform.position;
            Vector3[] newPath = new Vector3[path.Length + 2];

            if (CarDirection.x != 0)
            {
                position.x = border.x;
            }

            else if (CarDirection.z != 0)
            {
                position.z = border.z;
            }

            newPath[0] = position;

            Vector3 pos = path[0];
            pos.y = position.y;
            newPath[1] = position + (position - pos).normalized * pathDistanceDelay;

            float dist = Vector3.Distance(position, _currDetectableObjectData.DetectableTransform.position);
            dist += Vector3.Distance(newPath[0], newPath[1]);

            for (int i = 0; i < path.Length; i++)
            {
                newPath[i + 2] = path[i];
                newPath[i + 2].y = position.y;
                dist += Vector3.Distance(newPath[i], newPath[i + 1]);
            }

            _currDetectableObjectData.DetectableTransform.DOPath(newPath, duration * dist, PathType.CatmullRom)
                .SetLookAt(0.01f)
                .OnStart((() =>
                {
                    SwipeDetectorManager.Instance.RemoveDetectableObject(_currDetectableObjectData);
                }))
                .OnComplete((() => _currDetectableObjectData.DetectableTransform.gameObject.SetActive(false)))
                .SetEase(curve);
        }

        public void HitMove(Vector3 hitPoint, float duration, Vector3 hitDistance, Vector3 hitBackTab, AnimationCurve curve)
        {
            Vector3 position = _currDetectableObjectData.DetectableTransform.position;
            Vector3[] points = new Vector3[2];

            switch (MovementDirection.x)
            {
                case > 0:
                {
                    position.x = hitPoint.x - _currDetectableObjectData.DetectableTransform.localScale.x + hitDistance.x;
                    points[1] = position;
                    points[1].x -= hitBackTab.x;
                    break;
                }
                case < 0:
                {
                    position.x = hitPoint.x + _currDetectableObjectData.DetectableTransform.localScale.x - hitDistance.x;
                    points[1] = position;
                    points[1].x += hitBackTab.x;
                    break;
                }
                default:
                {
                    switch (MovementDirection.z)
                    {
                        case > 0:
                            position.z = hitPoint.z - _currDetectableObjectData.DetectableTransform.localScale.z -
                                         hitDistance.z;
                            points[1] = position;
                            points[1].z -= hitBackTab.z;
                            break;
                        case < 0:
                            position.z = hitPoint.z + _currDetectableObjectData.DetectableTransform.localScale.z +
                                         hitDistance.z;
                            points[1] = position;
                            points[1].z += hitBackTab.z;
                            break;
                    }

                    break;
                }
            }

            points[0] = position;
            float dist = Vector3.Distance(position, _currDetectableObjectData.DetectableTransform.position);

            _currDetectableObjectData.DetectableTransform.DOPath(points, duration * dist).SetEase(curve);
        }

        public void OnDestroyed()
        {
            _currDetectableObjectData.DetectableTransform.gameObject.SetActive(false);
        }

        private void SendManualRaycast()
        {
            RayHitDetectorManager.DetectObjects(_currDetectableObjectData.DetectableTransform, _manualRay, this);
        }
    }
}