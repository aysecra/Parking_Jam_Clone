using System.Collections.Generic;
using UnityEngine;

namespace ParkingJamClone.Pattern
{
    public abstract class ObjectPool : MonoBehaviour
    {
        [SerializeField] protected Transform parentObject;
        [SerializeField] protected uint amountToPool;
        [SerializeField] protected bool isAllObjectActive;
        
        protected List<GameObject> _objects;
        public List<GameObject> Objects => _objects;

        
        public uint AmountToPool
        {
            get => amountToPool;
            set => amountToPool = value;
        }
        
        protected virtual void Start()
        {
            for(int i = 0; i < amountToPool; i++)
            {
                AddNewObject();
            }
        }

        protected abstract void AddNewObject();
        public abstract GameObject GetObject();
    }
}
