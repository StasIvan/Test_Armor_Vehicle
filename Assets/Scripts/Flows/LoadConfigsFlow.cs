using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using Interfaces.ManagerInterfaces;
using Zenject;

namespace Flows
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