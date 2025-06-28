using Base;
using Cysharp.Threading.Tasks;
using Flows;
using Installers;
using Interfaces;
using Managers.Base;
using Zenject;

namespace Managers
{
    public class ContentLoaderManager : BaseManager
    {
        private readonly DiContainer _container;
        private readonly SignalBus _signalBus;

        public ContentLoaderManager(DiContainer container, SignalBus signalBus)
        {
            _container = container;
            _signalBus = signalBus;
        }
        
        public override void Initialize()
        {
            _signalBus.Subscribe<ChangeGameStateSignal>(OnChangeGameState);
            StartLoadContent().Forget();
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<ChangeGameStateSignal>(OnChangeGameState);
        }
        
        private void OnChangeGameState(ChangeGameStateSignal gameState)
        {
            if (gameState.State != GameState.Loading) return;

            SpawnGameObjects().Forget();
        }
        
        private async UniTask StartLoadContent()
        {
            IFlowRunner startFlow = _container.Instantiate<LoadConfigsFlow>();
            await startFlow.ExecuteFlow();

            await SpawnGameObjects();
            
            _signalBus.Fire<OnContentLoadedSignal>();
        }
        
        private async UniTask SpawnGameObjects()
        {
            IFlowRunner firstSpawnFlow = _container.Instantiate<SpawnGameObjectsFlow>();
            await firstSpawnFlow.ExecuteFlow();
            _signalBus.Fire<OnGameItemsSpawnedSignal>();
        }
        
    }
}