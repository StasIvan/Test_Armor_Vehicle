using System.Collections.Generic;
using System.Linq;
using Base;
using GameItems;
using Interfaces;
using Pool;
using UnityEngine;

namespace Managers
{
    public class SpawnManager : BaseManager, ISpawner
    {
        private readonly IPool _pool;
        private List<BaseGameItem> _poolItems;

        public SpawnManager(IPool pool)
        {
            _pool = pool;
        }

        public override void Initialize()
        {
            _poolItems = new();
        }

        public override void Dispose()
        {
            _poolItems.Clear();
        }

        public T GetItem<T>(Vector3 position, Quaternion rotation) where T : BaseGameItem
        {
            var item = _pool.GetMonoBehaviour<T>(position, rotation);
            //item.Initialize(position, rotation);
            _poolItems.Add(item);

            return item.GetComponent<T>();
        }

        public void ReleaseAll()
        {
            _pool.ReleaseAll();
            _poolItems.Clear();
        }

        public void Release<T>(T component) where T : BaseGameItem
        {
            _pool.Release(component);
            _poolItems.Remove(component);
        }

        public void ReleaseAllComponents<T>() where T : Component
        {
            var items = _poolItems.Where(v => v as T != null).ToArray();
            
            _pool.ReleaseAllComponents<T>();
            
            foreach (var item in items)
            {
                _poolItems.Remove(item);
            }
        }
    }

    public enum ItemType
    {
        None,
        Player,
        Enemy,
        Bullet
    }
}