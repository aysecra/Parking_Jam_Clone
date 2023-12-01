using System;
using System.Collections.Generic;
using ParkingJamClone.Interface;

namespace ParkingJamClone.Logic
{
    public static class EventManager
    {
        private static Dictionary<Type, List<IEventListener>> _listenerDictionary;
        
        public static void EventStartListening<T>(this EventListener<T> listener) where T : struct
        {
            AddEventListener<T>(listener);
        }
        
        public static void EventStopListening<T>(this EventListener<T> listener) where T : struct
        {
            RemoveEventListener<T>(listener);
        }
        
        public static void TriggerEvent<T>(T triggerEvent) where T : struct
        {
            TriggerListenerEvent(triggerEvent);
        }
        
        #region Listener Dictionary Elements
        private static void AddEventListener<T>(EventListener<T> listener) where T : struct
        {
            _listenerDictionary ??= new Dictionary<Type, List<IEventListener>>();
        
            if (!_listenerDictionary.ContainsKey(typeof(T)))
            {
                _listenerDictionary[typeof(T)] = new List<IEventListener>();
            }
        
            _listenerDictionary[typeof(T)].Add(listener);
        }
        
        private static void RemoveEventListener<T>(EventListener<T> listener) where T : struct
        {
            if (_listenerDictionary.ContainsKey(typeof(T)))
            {
                List<IEventListener> listenerList = _listenerDictionary[typeof(T)];
        
                for (int i = 0; i < listenerList.Count; i++)
                {
                    if (listener.Equals(listenerList[i]))
                    {
                        listenerList.Remove(listenerList[i]);
        
                        if (listenerList.Count == 0)
                            _listenerDictionary.Remove(typeof(T));
                        
                        break;
                    }
                }
            }
        }
        
        private static void TriggerListenerEvent<T>(T triggerEvent) where T : struct
        {
            if (_listenerDictionary.ContainsKey(typeof(T)))
            {
                List<IEventListener> listenerList = _listenerDictionary[typeof(T)];

                foreach (var listener in listenerList)
                {
                    ((listener as EventListener<T>)!).OnEventTrigger(triggerEvent);
                }
            }
        }
        #endregion
    }
}
