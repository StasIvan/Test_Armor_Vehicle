using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Core.Flows
{
    public class LoadConfigsFlow : IFlowRunner
    {
        private IConfigManager _configManager;
        private IWindowManager _windowManager;

        [Inject]
        public void Construct(IConfigManager configManager, IWindowManager windowManager)
        {
            _configManager = configManager;
            _windowManager = windowManager;
        }

        public async UniTask ExecuteFlow()
        {
            await _configManager.LoadConfigs();
            await _windowManager.LoadWindowContent();
        }
    }
}