using Base;
using Cysharp.Threading.Tasks;
using Flows;
using Installers;
using Interfaces;
using Zenject;

namespace Managers
{
    public class ContentLoaderManager : BaseManager
    {
        private readonly DiContainer _container;
        private readonly IStateManager _stateManager;

        public ContentLoaderManager(DiContainer container, IStateManager stateManager)
        {
            _container = container;
            _stateManager = stateManager;
        }
        
        public override void Initialize()
        {
            StartLoadContent().Forget();
        }

        public override void Dispose()
        {
        }
        
        private async UniTask StartLoadContent()
        {
            _stateManager.ChangeState(GameState.Loading);
            
            IFlowRunner startFlow = _container.Instantiate<LoadConfigsFlow>();
            await startFlow.ExecuteFlow();

            IFlowRunner firstSpawnFlow = _container.Instantiate<FirstSpawnFlow>();
            await firstSpawnFlow.ExecuteFlow();
            
            _stateManager.ChangeState(GameState.Start);
        }
    }
}