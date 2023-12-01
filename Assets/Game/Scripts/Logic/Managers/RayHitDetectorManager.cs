using System.Collections.Generic;
using System.Linq;
using ParkingJamClone.Data;
using ParkingJamClone.Structs;
using UnityEngine;

namespace ParkingJamClone.Logic
{
    public static class RayHitDetectorManager
    {
        private static List<DetectableObjectData> _detectableObjects = new List<DetectableObjectData>();

        public static void AddDetectableObject(DetectableObjectData detectableObject)
        {
            if (!_detectableObjects.Contains(detectableObject))
            {
                _detectableObjects.Add(detectableObject);
            }
        }

        public static void RemoveDetectableObject(DetectableObjectData detectableObject)
        {
            if (_detectableObjects.Contains(detectableObject))
            {
                _detectableObjects.Remove(detectableObject);
            }
        }

        public static void ClearList()
        {
            _detectableObjects.Clear();
        }

        public static void DetectObjects(Transform detectableTransform, ScreenToWorldPointData manualRay,
            SwipeableCar car)
        {
            if (_detectableObjects.Count <= 0) return;
            bool isDetected = false;

            DetectableObjectData closestObject = default;
            float minDistance = 0;


            foreach (var detectableObject in _detectableObjects.Where(detectableObject =>
                         detectableObject.DetectableTransform != detectableTransform &&
                         ObjectDetector.CalculateIsHitToObject(detectableObject, manualRay)))
            {
                if (isDetected)
                {
                    float dist = Vector3.Distance(manualRay.Origin, detectableObject.DetectableTransform.position);
                    if (!(dist < minDistance)) continue;
                    closestObject = detectableObject;
                    minDistance = Vector3.Distance(manualRay.Origin,
                        detectableObject.DetectableTransform.position);
                }
                else
                {
                    closestObject = detectableObject;
                    minDistance = Vector3.Distance(manualRay.Origin, detectableObject.DetectableTransform.position);
                    isDetected = true;
                }
            }

            if (isDetected)
            {
                (closestObject.DetectableScript as DetectedHittableObject)!.HittedCar = car;
                closestObject.DetectableScript.OnDetected();
            }
            else
            {
                CarMovement.Instance.GoToPath(car);
            }
        }
    }
}