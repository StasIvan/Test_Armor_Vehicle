using Configs.BulletConfig;
using Configs.EnemyConfig;
using GameItems.Base;
using GameItems.BulletItem;
using GameItems.EnemyItem;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Managers.SpawnManager.ItemControllerFactories
{
    public class BulletControllerFactory : IItemControllerFactory
    {
        private readonly IPool _pool;
        private readonly IConfigManager _configManager;
        private readonly DiContainer _container;

        public BulletControllerFactory(IPool pool, IConfigManager configManager, DiContainer container)
        {
            _pool = pool;
            _configManager = configManager;
            _container = container;
        }
        
        public GameItemType Type { get => GameItemType.Bullet; }
        public IItemController Create(Vector3 position, Quaternion rotation)
        {
            var item = _pool.GetMonoBehaviour<BulletView>(position, rotation);
            item.Initialize();
            
            BulletConfig config = _configManager.GetConfig<BulletConfigs, BulletConfig>();


            var model = new BulletModel()
            {
                Damage = config.damage,
                Speed = config.speed,
            };

            item.Bind(model);
            
            BulletController controller = _container.Instantiate<BulletController>();
            controller.Initialize();
            controller.Bind(item, model);
            
            return controller;
        }


        public void Delete(IPool pool, IItemController controller)
        {
            var view = controller.GetView<BulletView>();
            view.Dispose();
            controller.Dispose();
            pool.Release(view);
        }
    }
}