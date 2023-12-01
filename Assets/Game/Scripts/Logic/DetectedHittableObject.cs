using ParkingJamClone.Data;
using ParkingJamClone.Interfaces;
using ParkingJamClone.Structs;
using UnityEngine;

namespace ParkingJamClone.Logic
{
    public class DetectedHittableObject : IDetectable
    {
        private readonly DetectableObjectData _currDetectableObjectData;
        private readonly Vector3 _minBound;
        private readonly Vector3 _maxBound;
        public SwipeableCar HittedCar;

        public DetectedHittableObject(DetectableObjectData objectData)
        {
            _currDetectableObjectData = objectData;
            _minBound = Calculator.LocalPointToWorld(_currDetectableObjectData.Mesh.bounds.min,
                _currDetectableObjectData.DetectableTransform);

            _maxBound = Calculator.LocalPointToWorld(_currDetectableObjectData.Mesh.bounds.max,
                _currDetectableObjectData.DetectableTransform);
        }

        public void OnDetected()
        {
            // Debug.Log("ray detect this car: " + _currDetectableObjectData.DetectableTransform.name);
            CalculateHitPoint();
        }

        private void CalculateHitPoint()
        {
            Vector3 hitPoint = new Vector3(0, 0, _currDetectableObjectData.DetectableTransform.position.z);

            switch (HittedCar.MovementDirection.x)
            {
                case > 0:
                    hitPoint.x = _minBound.x;
                    break;
                case < 0:
                    hitPoint.x = _maxBound.x;
                    break;
                default:
                {
                    if (HittedCar.MovementDirection.z > 0)
                    {
                        hitPoint.z = _minBound.z;
                    }
                    else if (HittedCar.MovementDirection.z < 0)
                    {
                        hitPoint.z = _maxBound.z;
                    }

                    break;
                }
            }

            CarMovement.Instance.GoToHitPoint(hitPoint, HittedCar);
        }
    }
}