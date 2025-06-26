using System;
using GameItems;
using UnityEngine;

namespace Pool
{
    public class PoolItem : MonoBehaviour, IDisposable
    {
        [SerializeField] private PoolType _type;

        public PoolType Type
        {
            get => _type;
        }

        public void OnSpawned(Vector3 position, Quaternion rotation, Transform parent)
        {
            transform.SetParent(parent);
            transform.position = position;
            transform.rotation = rotation;
            gameObject.SetActive(true);
        }

        public void Dispose()
        {
            gameObject.GetComponent<BaseGameItem>().Dispose();
            gameObject.SetActive(false);
        }

    }
}