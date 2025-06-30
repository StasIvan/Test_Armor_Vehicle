using System;
using System.Linq;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Features.Configs.Base;

namespace Features.Configs.PlayerConfig
{
    public class PlayerConfigs : BaseConfigs<PlayerConfigSO, PlayerConfig>, IConfigGetter
    {
        private PlayerConfig[] _playerConfigs;
        private readonly string _searchingWord = "PlayerConfigs";
        
        public async UniTask InitAsync()
        {
            var loader = new ConfigsLoader();
            SetValues(await loader.LoadConfigs<PlayerConfigSO>(_searchingWord));
        }

        public override void SetValues(PlayerConfigSO[] items)
        {
            valuesSetter.SetValues(items, out _playerConfigs);
        }

        public BaseConfig GetConfig(int id = 0)
        {
            PlayerConfig config = _playerConfigs.FirstOrDefault(v => v.id == id);

            if (config == null) throw new NullReferenceException($"You're trying to get a non-existent Player_{id}");
            
            return config;
        }

        
    }
}