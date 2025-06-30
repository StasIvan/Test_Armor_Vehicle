using System;
using System.Linq;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Features.Configs.Base;

namespace Features.Configs.BulletConfig
{
    public class BulletConfigs: BaseConfigs<BulletConfigSO, BulletConfig>, IConfigGetter
    {
        private BulletConfig[] _bulletConfigs;
        private readonly string _searchingWord = "BulletConfigs";
        
        public async UniTask InitAsync()
        {
            var loader = new ConfigsLoader();
            SetValues(await loader.LoadConfigs<BulletConfigSO>(_searchingWord));
        }

        public override void SetValues(BulletConfigSO[] items)
        {
            valuesSetter.SetValues(items, out _bulletConfigs);
        }

        public BaseConfig GetConfig(int id = 0)
        {
            BulletConfig config = _bulletConfigs.FirstOrDefault(v => v.id == id);

            if (config == null) throw new NullReferenceException($"You're trying to get a non-existent Player_{id}");
            
            return config;
        }
    }
}