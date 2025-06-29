using Configs.BulletConfig;
using Configs.PlayerConfig;
using GameItems.Base;
using GameItems.BulletItem;
using GameItems.PlayerItems;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Managers.SpawnManager.ItemControllerFactories
{
    public class PlayerControllerFactory : IItemControllerFactory
    {
        private readonly IPool _pool;
        private readonly IConfigManager _configManager;
        private readonly DiContainer _container;

        public GameItemType Type { get => GameItemType.Player; }
        
        public PlayerControllerFactory(IPool pool, IConfigManager configManager, DiContainer container)
        {
            _pool = pool;
            _configManager = configManager;
            _container = container;
        }
        
        public IItemController Create(Vector3 position, Quaternion rotation)
        {
            var item = _pool.GetMonoBehaviour<PlayerView>(position, rotation);
            item.Initialize();
            
            PlayerConfig config = _configManager.GetConfig<PlayerConfigs, PlayerConfig>();
            
            var model = new PlayerModel()
            {
                Health = config.health,
                MaxHealth = config.health,
                Position = position,
                Speed = config.speed,
            };
            
            item.Bind(model);

            PlayerController controller = _container.Instantiate<PlayerController>();
            controller.Initialize();
            controller.Bind(item, model);
            
            return controller;
        }


        public void Delete(IPool pool, IItemController controller)
        {
            var view = controller.GetView<PlayerView>();
            view.Dispose();
            controller.Dispose();
            pool.Release(view);
        }
    }
}