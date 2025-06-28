using System;
using System.Linq;
using Configs.Base;
using Cysharp.Threading.Tasks;
using Interfaces;

namespace Configs.EnemyConfig
{
    public class EnemyConfigs : BaseConfigs<EnemyConfigSO, EnemyConfig>, IConfigGetter
    {
        private EnemyConfig[] _enemyConfigs;
        private readonly string _searchingWord = "EnemyConfigs";
        
        public async UniTask InitAsync()
        {
            var loader = new ConfigsLoader();
            SetValues(await loader.LoadConfigs<EnemyConfigSO>(_searchingWord));
        }

        public override void SetValues(EnemyConfigSO[] items)
        {
            valuesSetter.SetValues(items, out _enemyConfigs);
        }

        public BaseConfig GetConfig(int id = 0)
        {
            EnemyConfig config = _enemyConfigs.FirstOrDefault(v => v.id == id);

            if (config == null) throw new NullReferenceException($"You're trying to get a non-existent Player_{id}");
            
            return config;
        }

    }
}