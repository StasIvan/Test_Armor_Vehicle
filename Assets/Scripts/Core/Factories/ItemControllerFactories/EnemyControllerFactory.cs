using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Features.Configs.EnemyConfig;
using Features.GameItems.Base;
using Features.GameItems.EnemyItem;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Factories.ItemControllerFactories
{
    public class EnemyControllerFactory : IItemControllerFactory, IDisposable
    {
        private readonly IPool _pool;
        private readonly IConfigManager _configManager;
        private readonly DiContainer _container;

        private Dictionary<int, FactoryContainer> _containers = new();
        
        public EnemyControllerFactory(IPool pool, IConfigManager configManager, DiContainer container)
        {
            _pool = pool;
            _configManager = configManager;
            _container = container;
        }
        
        public GameItemType Type { get => GameItemType.Enemy; }
        
        public IItemController Create(Vector3 position, Quaternion rotation)
        {
            var item = _pool.GetMonoBehaviour<EnemyView>(position, rotation);
            EnemyConfig config = _configManager.GetConfig<EnemyConfigs, EnemyConfig>();

            if (!_containers.TryGetValue(item.GetHashCode(), out var container))
            {
                container = CreateNewContainer(item, config, position, rotation);
                _containers.TryAdd(item.GetHashCode(), container);
            }
            else
            {
                UpdateModelProperties((EnemyModel)container.Model, config, position, rotation);
            }

            InitializeItemAndController(item, (EnemyController)container.Controller);

            return (EnemyController)container.Controller;
        }

        private FactoryContainer CreateNewContainer(EnemyView item, EnemyConfig config, Vector3 position, Quaternion rotation)
        {
            var model = new EnemyModel
            {
                Health = new ReactiveProperty<float>(config.health),
                MaxHealth = new ReactiveProperty<float>(config.health),
                Damage = new ReactiveProperty<float>(config.damage),
                Speed = new ReactiveProperty<float>(config.speed),
                RotationSpeed = new ReactiveProperty<float>(config.rotationSpeed),
                Position = new ReactiveProperty<Vector3>(position),
                Rotation = new ReactiveProperty<Quaternion>(rotation),
            };

            item.Bind(model);

            var controller = _container.Instantiate<EnemyController>();
            controller.Bind(item, model);

            return new FactoryContainer
            {
                Model = model,
                Controller = controller
            };
        }

        private void UpdateModelProperties(EnemyModel model, EnemyConfig config, Vector3 position, Quaternion rotation)
        {
            model.Health.Value = config.health;
            model.MaxHealth.Value = config.health;
            model.Damage.Value = config.damage;
            model.Speed.Value = config.speed;
            model.RotationSpeed.Value = config.rotationSpeed;
            model.Position.Value = position;
            model.Rotation.Value = rotation;
        }

        private void InitializeItemAndController(EnemyView item, EnemyController controller)
        {
            item.Initialize();
            controller.Initialize();
        }


        public void Delete(IPool pool, IItemController controller)
        {
            var view = controller.GetView<EnemyView>();
            controller.Dispose();
            pool.Release(view);
        }
        
        public void Dispose()
        {
            _containers.Clear();
        }
    }
}