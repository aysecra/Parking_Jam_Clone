using ParkingJamClone.Interfaces;
using UnityEngine;

namespace ParkingJamClone.Structs
{
    public struct DetectableObjectData
    {
        public Mesh Mesh;
        public Transform DetectableTransform;
        public IDetectable DetectableScript;
    }
}