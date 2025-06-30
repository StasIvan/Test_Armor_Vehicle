﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Core.Managers.Base;
using Cysharp.Threading.Tasks;
using Features.Configs;
using Features.Configs.BulletConfig;
using Features.Configs.EnemyConfig;
using Features.Configs.GameConfig;
using Features.Configs.PlayerConfig;

namespace Core.Managers
{
    public class ConfigsManager : BaseManager, IConfigManager
    {
        private List<IConfigGetter> _configs = new();

        public override void Initialize()
        {
        }

        public override void Dispose()
        {
            _configs.Clear();
        }

        public TV GetConfig<T, TV>(int id = 0) where T : IConfigGetter where TV : BaseConfig
        {
            var item = _configs.OfType<T>().FirstOrDefault();

            if (item == null) throw new NullReferenceException();

            return item.GetConfig(id) as TV;
        }

        public async UniTask LoadConfigs()
        {
            PlayerConfigs playerConfigs = new PlayerConfigs();
            EnemyConfigs enemyConfigs = new EnemyConfigs();
            BulletConfigs bulletConfigs = new BulletConfigs();
            GameConfigs gameConfigs = new GameConfigs();

            _configs.Add(playerConfigs);
            _configs.Add(enemyConfigs);
            _configs.Add(bulletConfigs);
            _configs.Add(gameConfigs);

            var tasks = new[]
            {
                playerConfigs.InitAsync(),
                enemyConfigs.InitAsync(),
                bulletConfigs.InitAsync(),
                gameConfigs.InitAsync(),
            };

            await UniTask.WhenAll(tasks);
        }
    }
}