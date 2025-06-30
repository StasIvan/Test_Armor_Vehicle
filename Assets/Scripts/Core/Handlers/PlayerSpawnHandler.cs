using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Cysharp.Threading.Tasks;
using Features.GameItems.Base;
using Features.GameItems.PlayerItems;
using UnityEngine;

namespace Core.Handlers
{
    public class PlayerSpawnHandler : IHandler
    {
        private readonly ISpawner _spawner;
        private readonly ISettable<PlayerController> _carSetter;

        public PlayerSpawnHandler(ISpawner spawner, ISettable<PlayerController> carSetter)
        {
            _spawner = spawner;
            _carSetter = carSetter;
        }
        
        public UniTask Execute()
        {
            _spawner.ReleaseAllComponents<PlayerController>();
            var item =
                _spawner.GetItem(GameItemType.Player, Vector3.zero, Quaternion.Euler(0f, 180f, 0f)) as PlayerController;
            _carSetter.Set(item);
            return UniTask.CompletedTask;
        }
    }
}