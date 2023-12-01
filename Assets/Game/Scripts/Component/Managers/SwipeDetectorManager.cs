using System.Collections.Generic;
using ParkingJamClone.Components;
using ParkingJamClone.Components.Manager;
using ParkingJamClone.Enums;
using ParkingJamClone.Interface;
using ParkingJamClone.Logic;
using ParkingJamClone.Structs;
using ParkingJamClone.Structs.Event;
using UnityEngine;

namespace ParkingJamClone.Manager
{
    public class SwipeDetectorManager : Singleton<SwipeDetectorManager>
        , EventListener<InputEvent>
    {
        private List<DetectableObjectData> _detectableObjects = new List<DetectableObjectData>();
        private DetectableObjectData _touchedObject;
        private bool _isTouched;

        public void AddDetectableObject(DetectableObjectData detectableObject)
        {
            if (!_detectableObjects.Contains(detectableObject))
            {
                _detectableObjects.Add(detectableObject);
            }
        }

        public void RemoveDetectableObject(DetectableObjectData detectableObject)
        {
            int index = -1;
            for (int i = 0; i < _detectableObjects.Count; i++)
            {
                if (detectableObject.DetectableTransform == _detectableObjects[i].DetectableTransform)
                    index = i;
            }

            if (index >= 0)
                _detectableObjects.RemoveAt(index);
            
            if (_detectableObjects.Count == 0)
            {
                GameManager.Instance.SetLevelCompleted();
            }
        }

        private void DetectObjects(Vector3 screenPosition)
        {
            if (_detectableObjects.Count <= 0) return;

            foreach (var detectableObject in _detectableObjects)
            {
                ScreenToWorldPointData screenToWorldPointData =
                    CameraController.Instance.ScreenToWorldPoint(screenPosition);

                if (ObjectDetector.CalculateIsHitToObject(detectableObject, screenToWorldPointData))
                {
                    _touchedObject = detectableObject;
                    _isTouched = true;
                }
            }
        }

        private void OnEnable()
        {
            EventManager.EventStartListening<InputEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening<InputEvent>(this);
        }

        public void OnEventTrigger(InputEvent currentEvent)
        {
            switch (currentEvent.State)
            {
                case TouchState.Touch:
                    DetectObjects(currentEvent.Position);
                    break;
                case TouchState.End:
                {
                    if (_isTouched)
                    {
                        Vector3 hitPoint = ObjectDetector.GetHitPoint(_touchedObject.DetectableTransform,
                            CameraController.Instance.ScreenToWorldPoint(currentEvent.Position));
                        Vector3 diff = hitPoint - _touchedObject.DetectableTransform.position;

                        SwipeableCar car = (_touchedObject.DetectableScript as SwipeableCar);
                        Vector3 carDirection = car!.CarDirection;

                        if (carDirection.x != 0 && diff.x > 0) diff = Vector3.right;
                        if (carDirection.x != 0 && diff.x < 0) diff = Vector3.left;
                        if (carDirection.z != 0 && diff.z > 0) diff = Vector3.forward;
                        if (carDirection.z != 0 && diff.z < 0) diff = Vector3.back;

                        car?.SetRayDirection(diff);
                        _touchedObject.DetectableScript.OnDetected();
                    }

                    _isTouched = false;
                    break;
                }
            }
        }
    }
}