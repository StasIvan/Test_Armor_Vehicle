using System;
using System.Linq;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Features.Configs.Base;

namespace Features.Configs.GameConfig
{
    public class GameConfigs : BaseConfigs<GameConfigSO, GameConfig>, IConfigGetter
    {
        private GameConfig[] _playerConfigs;
        private readonly string _searchingWord = "GameConfigs";
        
        public async UniTask InitAsync()
        {
            var loader = new ConfigsLoader();
            SetValues(await loader.LoadConfigs<GameConfigSO>(_searchingWord));
        }

        public override void SetValues(GameConfigSO[] items)
        {
            valuesSetter.SetValues(items, out _playerConfigs);
        }

        public BaseConfig GetConfig(int id = 0)
        {
            GameConfig config = _playerConfigs.FirstOrDefault(v => v.id == id);

            if (config == null) throw new NullReferenceException($"You're trying to get a non-existent Player_{id}");
            
            return config;
        }
    }
}