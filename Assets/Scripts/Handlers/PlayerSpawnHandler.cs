using Cysharp.Threading.Tasks;
using GameItems.PlayerItems;
using Interfaces;
using UnityEngine;

namespace Handlers
{
    public class PlayerSpawnHandler : IHandler
    {
        private readonly ISpawner _spawner;
        private readonly IConfigManager _configManager;
        private readonly ISettable<CarItem> _carSetter;

        public PlayerSpawnHandler(ISpawner spawner, IConfigManager configManager, ISettable<CarItem> carSetter)
        {
            _spawner = spawner;
            _configManager = configManager;
            _carSetter = carSetter;
        }
        
        public UniTask Execute()
        {
            _spawner.ReleaseAllComponents<CarItem>();
            var item = _spawner.GetItem<CarItem>(Vector3.zero, Quaternion.Euler(0f, 180f, 0f));
            item.Initialize();
            _carSetter.Set(item);
            return UniTask.CompletedTask;
        }
    }
}