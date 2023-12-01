using ParkingJamClone.Enums;
using UnityEngine;

namespace ParkingJamClone.Structs.Event
{
    /// <summary>
    /// Event is triggered when tapping
    /// </summary>
    public struct InputEvent
    {
        public TouchState State { get; }
        public Vector3 Position { get; }

        public InputEvent(TouchState state, Vector3 position)
        {
            State = state;
            Position = position;
        }
    }
}
