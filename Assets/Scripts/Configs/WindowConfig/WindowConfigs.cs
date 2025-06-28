using System;
using System.Linq;
using Configs.Base;
using Cysharp.Threading.Tasks;
using Managers;

namespace Configs.WindowConfig
{
    public class WindowConfigs : BaseConfigs<WindowConfigSO, WindowConfig>
    {
        private WindowConfig[] _windowConfigs;
        private readonly string _searchingWord = "WindowConfigs";
        
        public async UniTask InitAsync()
        {
            var loader = new ConfigsLoader();
            SetValues(await loader.LoadConfigs<WindowConfigSO>(_searchingWord));
        }

        public override void SetValues(WindowConfigSO[] items)
        {
            valuesSetter.SetValues(items, out _windowConfigs);
        }

        public WindowConfig[] GetAllConfigs()
        {
            return _windowConfigs;
        }
    }
}