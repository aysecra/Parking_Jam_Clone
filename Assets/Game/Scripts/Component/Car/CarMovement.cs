using System.Collections.Generic;
using DG.Tweening;
using ParkingJamClone.Logic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ParkingJamClone.Data
{
    public class CarMovement : Singleton<CarMovement>
    {
        [SerializeField] private float _durationPerDistance;
        [SerializeField] private Vector3 _hitDistance;
        [SerializeField] private Vector3 _hitBackTab;
        [SerializeField] private float _pathDistanceDelay;
        [SerializeField] private AnimationCurve _moveCurve;
        [SerializeField] private AnimationCurve _hitCurve;
        [SerializeField] private List<Transform> _pathPointList = new List<Transform>();

        private Vector3[] _path;

        public void GoToPath(SwipeableCar car)
        {
            if (_pathPointList.Count < 4) return;
            Vector3 direction = car.MovementDirection;

            switch (direction.x)
            {
                case > 0:
                    CalculateNewPath(1);
                    car.Move(_pathPointList[1].position, _path, _durationPerDistance, _pathDistanceDelay, _moveCurve);
                    break;
                case < 0:
                    CalculateNewPath(3);
                    car.Move(_pathPointList[3].position, _path, _durationPerDistance, _pathDistanceDelay, _moveCurve);
                    break;
                default:
                    switch (direction.z)
                    {
                        case > 0:
                            CalculateNewPath(0);
                            car.Move(_pathPointList[0].position, _path, _durationPerDistance, _pathDistanceDelay,
                                _moveCurve);
                            break;
                        case < 0:
                            CalculateNewPath(2);
                            car.Move(_pathPointList[2].position, _path, _durationPerDistance, _pathDistanceDelay,
                                _moveCurve);
                            break;
                    }

                    break;
            }
        }

        public void GoToHitPoint(Vector3 hitPoint, SwipeableCar car)
        {
            car.HitMove(hitPoint, _durationPerDistance, _hitDistance, _hitBackTab, _hitCurve);
        }

        private void CalculateNewPath(int index)
        {
            if (_pathPointList.Count <= 0) return;
            _path = new Vector3[_pathPointList.Count - index];
            int counter = 0;

            for (int i = index; i < _pathPointList.Count; i++)
            {
                _path[counter] = _pathPointList[i].position;
                counter++;
            }
        }
    }
}