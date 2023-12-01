using ParkingJamClone.Interfaces;
using ParkingJamClone.Logic;
using ParkingJamClone.Manager;
using ParkingJamClone.Structs;
using UnityEngine;

namespace ParkingJamClone.Components
{
    public class SwipeDetectableArea : DetectableArea
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
                SwipeableCar newSwipeableCar = new SwipeableCar(newObject);
                newObject.DetectableScript = newSwipeableCar;
            }

            SwipeDetectorManager.Instance.AddDetectableObject(newObject);
        }
    }
}