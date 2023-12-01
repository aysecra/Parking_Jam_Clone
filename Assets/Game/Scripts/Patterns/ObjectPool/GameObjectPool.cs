using System.Collections.Generic;
using UnityEngine;

namespace ParkingJamClone.Pattern
{
    public class GameObjectPool : ObjectPool
    {
        [SerializeField] protected GameObject childObject;
        
        public GameObject ChildObject => childObject;


        protected override void Start()
        {
            _objects = new List<GameObject>();
            base.Start();
        }
        
        protected override void AddNewObject()
        {
            GameObject newObject = Instantiate(childObject, parentObject);
            newObject.SetActive(isAllObjectActive);
            _objects.Add(newObject);
        }
        
        public override GameObject GetObject()
        {
            for(int i = 0; i < amountToPool; i++)
            {
                if(!_objects[i].activeInHierarchy)
                {
                    return _objects[i];
                }
            }
            
            AddNewObject();
            return _objects[^1];
        }
    }
}
