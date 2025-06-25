using System;
using UnityEngine;
using Zenject;

namespace Pool
{
    public class PoolItem : MonoBehaviour, IPoolable<Vector3, IMemoryPool>, IDisposable
    {
        [SerializeField] private PoolType _type;

        public PoolType Type
        {
            get => _type;
        }

        private IMemoryPool _pool;

        public void OnSpawned(Vector3 position, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
            gameObject.SetActive(true);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
            _pool = null;
        }

        public void Dispose()
        {
            _pool?.Despawn(this);
        }

    }
}