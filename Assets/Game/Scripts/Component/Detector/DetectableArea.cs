using System.Collections;
using ParkingJamClone.Pattern;
using UnityEngine;

namespace ParkingJamClone.Components
{
    public abstract class DetectableArea : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(IDelayedStart());
        }

        IEnumerator IDelayedStart()
        {
            yield return new WaitForSeconds(.1f);
            if (gameObject.TryGetComponent(out ObjectPool pool))
                GetDetectableObjectsFromPool(pool);
            else GetDetectableObjectsFromTransform();
        }

        private void GetDetectableObjectsFromPool(ObjectPool pool)
        {
            foreach (GameObject obj in pool.Objects)
            {
                GetObject(obj.transform);
            }
        }

        private void GetDetectableObjectsFromTransform()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                    GetObject(child);
            }
        }

        protected abstract void GetObject(Transform objTransform);
    }
}