using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Features.Configs.PlayerConfig;
using Features.GameItems.Base;
using Features.GameItems.PlayerItems;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Factories.ItemControllerFactories
{
    public class PlayerControllerFactory : IItemControllerFactory, IDisposable
    {
        private readonly IPool _pool;
        private readonly IConfigManager _configManager;
        private readonly DiContainer _container;

        public GameItemType Type { get => GameItemType.Player; }
        
        private Dictionary<int, FactoryContainer> _containers = new();
        
        public PlayerControllerFactory(IPool pool, IConfigManager configManager, DiContainer container)
        {
            _pool = pool;
            _configManager = configManager;
            _container = container;
        }
        
        public IItemController Create(Vector3 position, Quaternion rotation)
        {
            var item = _pool.GetMonoBehaviour<PlayerView>(position, rotation);
            PlayerConfig config = _configManager.GetConfig<PlayerConfigs, PlayerConfig>();

            if (!_containers.TryGetValue(item.GetHashCode(), out var container))
            {
                container = CreateNewContainer(item, config, position);
                _containers.TryAdd(item.GetHashCode(), container);
            }
            else
            {
                UpdateModelProperties((PlayerModel)container.Model, config, position, rotation);
            }

            InitializeItemAndController(item, (PlayerController)container.Controller);

            return (PlayerController)container.Controller;
        }

        private FactoryContainer CreateNewContainer(PlayerView item, PlayerConfig config, Vector3 position)
        {
            var model = new PlayerModel()
            {
                Health = new ReactiveProperty<float>(config.health),
                MaxHealth = new ReactiveProperty<float>(config.health),
                Position = new ReactiveProperty<Vector3>(position),
                Speed = new ReactiveProperty<float>(config.speed),
            };

            item.Bind(model);

            var controller = _container.Instantiate<PlayerController>();
            controller.Bind(item, model);

            return new FactoryContainer
            {
                Model = model,
                Controller = controller
            };
        }

        private void UpdateModelProperties(PlayerModel model, PlayerConfig config, Vector3 position, Quaternion rotation)
        {
            model.Health.Value = config.health;
            model.MaxHealth.Value = config.health;
            model.Speed.Value = config.speed;
            model.Position.Value = position;
        }

        private void InitializeItemAndController(PlayerView item, PlayerController controller)
        {
            item.Initialize();
            controller.Initialize();
        }


        public void Delete(IPool pool, IItemController controller)
        {
            var view = controller.GetView<PlayerView>();
            controller.Dispose();
            pool.Release(view);
        }
        
        public void Dispose()
        {
            _containers.Clear();
        }
    }
}