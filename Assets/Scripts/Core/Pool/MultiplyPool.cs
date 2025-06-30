using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Pool
{
    public class MultiplyPool
    {
        private readonly GameObject[] _prefabs;
        private readonly Transform _content;
        
        private readonly Dictionary<Type, HashSet<PoolItem>> _instances = new();
        private readonly Dictionary<Type, Queue<PoolItem>> _pool = new();
        private readonly DiContainer _diContainer;

        public MultiplyPool(GameObject[] prefabs, Transform content, DiContainer diContainer)
        {
            _prefabs = prefabs;
            _content = content;
            _diContainer = diContainer;
        }

        public T Get<T>(Vector3 position, Quaternion rotation) where T : Component
        {
            if (GetPool(typeof(T)).Count == 0)
            {
                PoolItem result = GetInstance<T>().AddComponent<PoolItem>();
                GetPool(typeof(T)).Enqueue(result);
            }

            var component = GetPool(typeof(T)).Dequeue().GetComponent<T>();
            PoolItem item = component.GetComponent<PoolItem>();
            if (component == null)
            {
                return null;
            }

            item.OnSpawned(position, rotation, _content);
            
            GetInstances(typeof(T)).Add(item);

            return component;
        }
        
        public void Release<T>(T component) where T : Component
        {
            var poolItem = component.gameObject.GetComponent<PoolItem>();
            var instance = GetInstances(typeof(T));
            Release<T>(poolItem, instance);
        }

        public void ReleaseAll()
        {
            foreach (var hashSet in _instances)
            {
                foreach (PoolItem instance in hashSet.Value)
                {
                    instance.Dispose();
                    GetPool(hashSet.Key).Enqueue(instance);
                }
            }

            _instances.Clear();
        }

        public void ReleaseAllComponents<T>() where T : Component
        {
            HashSet<PoolItem> instances = GetInstances(typeof(T));
            var itemsToRelease = new List<PoolItem>(instances);

            foreach (var item in itemsToRelease)
            {
                Release<T>(item, instances);
            }
        }

        public void Dispose()
        {
            ReleaseAll();
            
            foreach (var queue in _pool.Values)
            {
                foreach (PoolItem item in queue)
                {
                    GameObject.Destroy(item);
                }
            }

            _pool.Clear();
        }

        private void Release<T>(PoolItem poolItem, HashSet<PoolItem> instances) where T : Component
        {
            if (instances.Contains(poolItem))
            {
                poolItem.Dispose();
                GetPool(typeof(T)).Enqueue(poolItem);
                instances.Remove(poolItem);
            }
        }
        
        private GameObject GetInstance<T>() where T : Component
        {
            foreach (var prefab in _prefabs)
            {
                var component = prefab.gameObject.GetComponent(typeof(T));
                if (component != null)
                {
                    return _diContainer.InstantiatePrefab(prefab);
                }
            }

            return null;
        }
        
        private Queue<PoolItem> GetPool(Type type)
        {
            Queue<PoolItem> result;

            if (!_pool.TryGetValue(type, out result))
            {
                var newQueue = result = new Queue<PoolItem>();
                _pool.Add(type, newQueue);
            }

            return result;
        }

        private HashSet<PoolItem> GetInstances(Type type)
        {
            HashSet<PoolItem> result;

            if (!_instances.TryGetValue(type, out result))
            {
                var newHashSet = result = new HashSet<PoolItem>();
                _instances.Add(type, newHashSet);
            }

            return result;
        }
    }
}