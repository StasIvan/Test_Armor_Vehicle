using System.Threading.Tasks;
using Configs.GameConfig;
using Cysharp.Threading.Tasks;
using Handlers;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Flows
{
    public class SpawnGameObjectsFlow : IFlowRunner
    {
        private readonly DiContainer _container;
        
        public SpawnGameObjectsFlow(DiContainer container)
        {
            _container = container;
        }

        public async UniTask ExecuteFlow()
        {
            IHandler playerSpawnHandler = _container.Instantiate<PlayerSpawnHandler>();
            playerSpawnHandler.Execute();
            
            IHandler enemySpawnHandler = _container.Instantiate<EnemySpawnHandler>();
            
            await enemySpawnHandler.Execute();
        }

        
    }
}