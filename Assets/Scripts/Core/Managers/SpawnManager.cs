using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Core.Managers.Base;
using Features.GameItems.Base;
using UnityEngine;

namespace Core.Managers
{
    public class SpawnManager : BaseManager, ISpawner
    {
        private readonly IPool _pool;
        private List<IItemController> _poolItems;
        private readonly Dictionary<GameItemType, IItemControllerFactory> _factories;
        public SpawnManager(IPool pool, IEnumerable<IItemControllerFactory> factories)
        {
            _pool = pool;
            _factories = factories.ToDictionary(f => f.Type);
        }

        public override void Initialize()
        {
            _poolItems = new();
        }

        public override void Dispose()
        {
            ReleaseControllers(_poolItems);
            _pool.Dispose();
            _poolItems.Clear();
        }

        public IItemController GetItem(GameItemType type, Vector3 position, Quaternion rotation) 
        {
            if (!_factories.TryGetValue(type, out var factory))
                throw new InvalidOperationException($"Factory for {type} not found");

            var controller = factory.Create(position, rotation);
            _poolItems.Add(controller);
            return controller;
        }

        public void ReleaseAll()
        {
            ReleaseControllers(_poolItems);
            
            _pool.ReleaseAll();
            _poolItems.Clear();
        }

        public void Release<T>(T component) where T : IItemController
        {
            ReleaseController(component);

            _poolItems.Remove(component);
        }

        public void ReleaseAllComponents<T>() where T : class, IItemController
        {
            var items = _poolItems.Where(v => v is T).ToArray();

            ReleaseControllers(items.ToList());
            
            foreach (var item in items)
            {
                _poolItems.Remove(item);
            }
        }

        private void ReleaseControllers(List<IItemController> controllers)
        {
            foreach (var controller in controllers)
            {
                ReleaseController(controller);
            }
        }
        
        private void ReleaseController(IItemController controller)
        {
            if (!_factories.TryGetValue(controller.Type, out var factory))
                throw new InvalidOperationException($"Factory for {controller.Type} not found");

            factory.Delete(_pool, controller);
            // _poolItems.Remove(controller);
        }
    }
    
}