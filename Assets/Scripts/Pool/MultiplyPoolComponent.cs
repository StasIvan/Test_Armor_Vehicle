using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Pool
{
    public enum PoolType
    {
        None,
        Player,
        Enemy,
        Bullet
    }
    
    public class MultiplyPoolComponent : MonoBehaviour, IPool, IDisposable
    {
        [SerializeField] private GameObject[] _prefabs;
        [SerializeField] private Transform _content;
        
        private Dictionary<Type, HashSet<GameObject>> _instances;
        private Dictionary<Type, Queue<GameObject>> _pool;

        private void Awake()
        {
            _instances = new Dictionary<Type, HashSet<GameObject>>();
            _pool = new Dictionary<Type, Queue<GameObject>>();

            ReleaseAll();
        }

        public T GetMonoBehaviour<T>() where T : MonoBehaviour
        {
            return Get<T>();
        }

        private T Get<T>() where T : Component
        {
            if (GetPool(typeof(T)).Count == 0)
            {
                GameObject result = GetInstance<T>();
                GetPool(typeof(T)).Enqueue(result);
            }

            T component = GetPool(typeof(T)).Dequeue().GetComponent<T>();
            if (component == null)
            {
                return null;
            }

            var gameObject = component.gameObject;
            var transform = component.transform;
            if (transform.parent != _content)
                transform.SetParent(_content, false);

            GetInstances(typeof(T)).Add(gameObject);
            gameObject.SetActive(true);

            return component;
        }

        public GameObject GetInstance<T>() where T : Component
        {
            foreach (var prefab in _prefabs)
            {
                var component = prefab.gameObject.GetComponent(typeof(T));
                if (component != null)
                {
                    return Instantiate(prefab);
                }
            }

            return null;
        }

        public void Release<T>(T component) where T : Component
        {
            var go = component.gameObject;
            if (GetInstances(typeof(T)).Contains(go))
            {
                go.SetActive(false);
                
                GetPool(typeof(T)).Enqueue(go);
                GetInstances(typeof(T)).Remove(go);
            }
        }

        public void ReleaseAll()
        {
            foreach (var hashSet in _instances)
            {
                foreach (GameObject instance in hashSet.Value)
                {
                    instance.SetActive(false);
                    GetPool(hashSet.Key).Enqueue(instance);
                }
            }

            _instances.Clear();
        }

        public void Dispose()
        {
            ReleaseAll();

            foreach (var queue in _pool.Values)
            {
                foreach (GameObject gameObject in queue)
                {
                    GameObject.Destroy(gameObject);
                }
            }

            _pool.Clear();
        }

        private Queue<GameObject> GetPool(Type type)
        {
            Queue<GameObject> result;

            if (!_pool.TryGetValue(type, out result))
            {
                var newQueue = result = new Queue<GameObject>();
                _pool.Add(type, newQueue);
            }

            return result;
        }

        private HashSet<GameObject> GetInstances(Type type)
        {
            HashSet<GameObject> result;

            if (!_instances.TryGetValue(type, out result))
            {
                var newHashSet = result = new HashSet<GameObject>();
                _instances.Add(type, newHashSet);
            }

            return result;
        }
    }
}