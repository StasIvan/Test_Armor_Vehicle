using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Features.Configs.BulletConfig;
using Features.GameItems.Base;
using Features.GameItems.BulletItem;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Factories.ItemControllerFactories
{
    public class BulletControllerFactory : IItemControllerFactory, IDisposable
    {
        private readonly IPool _pool;
        private readonly IConfigManager _configManager;
        private readonly DiContainer _container;
        public GameItemType Type { get => GameItemType.Bullet; }
        
        private Dictionary<int, FactoryContainer> _containers = new();
        
        public BulletControllerFactory(IPool pool, IConfigManager configManager, DiContainer container)
        {
            _pool = pool;
            _configManager = configManager;
            _container = container;
        }
        
        public IItemController Create(Vector3 position, Quaternion rotation)
        {
            var item = _pool.GetMonoBehaviour<BulletView>(position, rotation);
            BulletConfig config = _configManager.GetConfig<BulletConfigs, BulletConfig>();

            if (!_containers.TryGetValue(item.GetHashCode(), out var container))
            {
                container = CreateNewContainer(item, config);
                _containers.TryAdd(item.GetHashCode(), container);
            }
            else
            {
                UpdateModelProperties((BulletModel)container.Model, config);
            }

            InitializeItemAndController(item, (BulletController)container.Controller);

            return (BulletController)container.Controller;
        }

        private FactoryContainer CreateNewContainer(BulletView item, BulletConfig config)
        {
            var model = new BulletModel
            {
                Direction = new ReactiveProperty<Vector3>(),
                Damage = new ReactiveProperty<float>(config.damage),
                Speed = new ReactiveProperty<float>(config.speed),
            };

            item.Bind(model);

            var controller = _container.Instantiate<BulletController>();
            controller.Bind(item, model);

            return new FactoryContainer
            {
                Model = model,
                Controller = controller
            };
        }

        private void UpdateModelProperties(BulletModel model, BulletConfig config)
        {
            model.Direction = new ReactiveProperty<Vector3>();
            model.Damage.Value = config.damage;
            model.Speed.Value = config.speed;
        }

        private void InitializeItemAndController(BulletView item, BulletController controller)
        {
            item.Initialize();
            controller.Initialize();
        }

        public void Delete(IPool pool, IItemController controller)
        {
            var view = controller.GetView<BulletView>();
            controller.Dispose();
            pool.Release(view);
        }

        public void Dispose()
        {
            _containers.Clear();
        }
    }
}