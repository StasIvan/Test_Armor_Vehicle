﻿using System.Threading.Tasks;
using Configs.GameConfig;
using Cysharp.Threading.Tasks;
using Handlers;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Flows
{
    public class FirstSpawnFlow : IFlowRunner
    {
        private readonly DiContainer _container;
        
        public FirstSpawnFlow(DiContainer container)
        {
            _container = container;
        }

        public UniTask ExecuteFlow()
        {
            IHandler enemySpawnHandler = _container.Instantiate<EnemySpawnHandler>();
            
            enemySpawnHandler.Execute();
            
            
            IHandler playerSpawnHandler = _container.Instantiate<PlayerSpawnHandler>();
            playerSpawnHandler.Execute();
            
            return UniTask.CompletedTask;
        }

        
    }
}