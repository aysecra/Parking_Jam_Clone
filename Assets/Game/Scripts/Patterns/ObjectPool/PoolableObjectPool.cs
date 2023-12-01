using System.Collections.Generic;
using UnityEngine;

namespace ParkingJamClone.Pattern
{
    public class PoolableObjectPool : ObjectPool
    {
        [SerializeField] protected PoolableObject poolableObject;
        
        protected List<PoolableObject> _pooledObjects;

        public PoolableObject PoolableObject => poolableObject;
        
        protected override void Start()
        {
            _pooledObjects = new List<PoolableObject>();
            _objects = new List<GameObject>();
            base.Start();
        }
        
        protected override void AddNewObject()
        {
            GameObject newObject = Instantiate(poolableObject.gameObject, parentObject);
            newObject.SetActive(isAllObjectActive);
            PoolableObject newPoolableObject = newObject.GetComponent<PoolableObject>();
            _pooledObjects.Add(newPoolableObject);
            _objects.Add(newObject);
        }

        public override GameObject GetObject()
        {
            for(int i = 0; i < amountToPool; i++)
            {
                if(!_pooledObjects[i].gameObject.activeInHierarchy)
                {
                    return _pooledObjects[i].gameObject;
                }
            }
            
            AddNewObject();
            return _pooledObjects[^1].gameObject;
        }

        public PoolableObject GetPooledObject()
        {
            for(int i = 0; i < amountToPool; i++)
            {
                if(!_pooledObjects[i].gameObject.activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }
            
            AddNewObject();
            return _pooledObjects[^1];
        }
    }
}