using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using Zenject;

namespace Flows
{
    public class LoadConfigsFlow : IFlowRunner
    {
        private IConfigManager _configManager;

        [Inject]
        public void Construct(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        public async UniTask ExecuteFlow()
        {
            await _configManager.LoadConfigs();
        }
    }
}