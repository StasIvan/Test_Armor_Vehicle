using System;
using UnityEngine;

namespace Core.Pool
{
    public class PoolItem : MonoBehaviour, IDisposable
    {
        public void OnSpawned(Vector3 position, Quaternion rotation, Transform parent)
        {
            transform.SetParent(parent);
            transform.position = position;
            transform.rotation = rotation;
            gameObject.SetActive(true);
        }

        public void Dispose()
        {
            if (this == null || gameObject == null) return;
            
            gameObject.GetComponent<IDisposable>()?.Dispose();
            gameObject.SetActive(false);
        }

    }
}