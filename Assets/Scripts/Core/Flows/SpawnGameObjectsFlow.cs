using Core.Handlers;
using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Core.Flows
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