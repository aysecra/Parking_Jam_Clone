using System.Collections;
using ParkingJamClone.Enums;
using ParkingJamClone.Interfaces;
using ParkingJamClone.Logic;
using ParkingJamClone.Manager;
using ParkingJamClone.Structs.Event;
using UnityEngine;

namespace ParkingJamClone.Components.Manager
{
    public class InputManager : Singleton<InputManager>
        , IUpdateListener
    {
        [SerializeField] private float detectInputDelay = .65f;
        
        private Vector3 _firstPosition;
        private Vector3 _endPosition;
        private bool _isTouchable = true;

        void IUpdateListener.ManagedUpdate()
        {
            if (_isTouchable)
            {
                if (Application.isEditor)
                    DetectMouseInput(0);

                if (Application.isMobilePlatform)
                    DetectTouchInput();
            }
        }

        public void SetInputDelay()
        {
            StartCoroutine(IDelayInput());
        }

        private IEnumerator IDelayInput()
        {
            _isTouchable = false;
            yield return new WaitForSeconds(detectInputDelay);
            _isTouchable = true;
        }

        private void DetectTouchInput()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 pos = touch.position;

                if (touch.phase == TouchPhase.Began)
                {
                    _firstPosition = pos;
                    EventManager.TriggerEvent(new InputEvent(TouchState.Touch, _firstPosition));
                }

                else if (touch.phase == TouchPhase.Ended)
                {
                    _endPosition = pos;
                    EventManager.TriggerEvent(new InputEvent(TouchState.End, _endPosition));
                }
            }
        }


        private void DetectMouseInput(uint mouseButtonData)
        {
            if (mouseButtonData <= 3)
            {
                int mouseButton = (int) mouseButtonData;

                if (Input.GetMouseButtonDown(mouseButton))
                {
                    _firstPosition = Input.mousePosition;
                    EventManager.TriggerEvent(new InputEvent(TouchState.Touch, _firstPosition));
                }

                if (Input.GetMouseButtonUp(mouseButton))
                {
                    _endPosition = Input.mousePosition;
                    EventManager.TriggerEvent(new InputEvent(TouchState.End, _endPosition));
                }
            }
        }

        private void OnEnable()
        {
            UpdateManager.Instance.AddListener(this);
        }

        private void OnDisable()
        {
            UpdateManager.Instance.RemoveListener(this);
        }
    }
}