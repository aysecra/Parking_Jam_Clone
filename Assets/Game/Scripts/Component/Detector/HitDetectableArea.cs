using ParkingJamClone.Interfaces;
using ParkingJamClone.Logic;
using ParkingJamClone.Structs;
using UnityEngine;

namespace ParkingJamClone.Components
{
    public class HitDetectableArea : DetectableArea
    {
        protected override void GetObject(Transform objTransform)
        {
            if (!objTransform.TryGetComponent(out MeshFilter meshFilter)) return;
            
            DetectableObjectData newObject = new DetectableObjectData()
            {
                DetectableTransform = objTransform,
                Mesh = meshFilter.mesh,
            };
            
            if (objTransform.TryGetComponent(out IDetectable detectable))
            {
                newObject.DetectableScript = detectable;
            }
            else
            {
                DetectedHittableObject newTouchableCar = new DetectedHittableObject(newObject);
                newObject.DetectableScript = newTouchableCar;
            }
            
            RayHitDetectorManager.AddDetectableObject(newObject);
        }
    }
}
