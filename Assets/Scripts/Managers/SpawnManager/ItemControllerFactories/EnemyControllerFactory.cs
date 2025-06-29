using Configs.EnemyConfig;
using GameItems.Base;
using GameItems.EnemyItem;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Managers.SpawnManager.ItemControllerFactories
{
    public class EnemyControllerFactory : IItemControllerFactory
    {
        private readonly IPool _pool;
        private readonly IConfigManager _configManager;
        private readonly DiContainer _container;

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
            item.Initialize();
            
            EnemyConfig config = _configManager.GetConfig<EnemyConfigs, EnemyConfig>();


            var model = new EnemyModel()
            {
                Health = config.health,
                MaxHealth = config.health,
                Damage = config.damage,
                Speed = config.speed,
                RotationSpeed = config.rotationSpeed,
                Position = position,
                Rotation = rotation,
            };

            item.Bind(model);
            
            EnemyController controller = _container.Instantiate<EnemyController>();
            controller.Initialize();
            controller.Bind(item, model);
            
            return controller;
        }

        public void Delete(IPool pool, IItemController controller)
        {
            var view = controller.GetView<EnemyView>();
            view.Dispose();
            controller.Dispose();
            pool.Release(view);
        }
    }
}