using System.Collections.Generic;
using Configs.GameConfig;
using GameItems.EnemyItem;
using Interfaces;
using Pool;
using UnityEngine;

namespace Handlers
{
    public class EnemySpawnHandler : IHandler
    {
        private readonly ISpawner _spawner;
        private readonly IConfigManager _configManager;
        private readonly int _attemptsMultiplier = 20;
        private List<EnemyItem> _items = new();
        private readonly float _minDistance = 2f;
        private readonly float _minDistanceSquared;

        public EnemySpawnHandler(ISpawner spawner, IConfigManager configManager)
        {
            _spawner = spawner;
            _configManager = configManager;

            _minDistanceSquared = _minDistance * _minDistance;
        }

        public void Execute()
        {
            _spawner.ReleaseAll();

            GameConfig config = _configManager.GetConfig<GameConfigs, GameConfig>();

            GetEnemies(config);
        }

        private void GetEnemies(GameConfig config)
        {
            int placed = 0;
            int attempts = 0;
            int maxAttempts = config.enemyCount * _attemptsMultiplier;

            while (placed < config.enemyCount && attempts < maxAttempts)
            {
                Vector2 randomCircle = Random.insideUnitCircle * config.levelSize;
                Vector3 candidatePosition = new Vector3(randomCircle.x, 0f, randomCircle.y) +
                                            Vector3.forward * (config.levelSize.y);
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                if (IsFarEnough(candidatePosition))
                {
                    var item = _spawner.GetItem<EnemyItem>(candidatePosition, rotation);
                    //item.Init(_managers, candidatePosition);
                    _items.Add(item);
                    placed++;
                }

                attempts++;
            }
        }

        private bool IsFarEnough(Vector3 position)
        {
            foreach (var enemy in _items)
            {
                if ((enemy.transform.position - position).sqrMagnitude < _minDistanceSquared)
                    return false;
            }

            return true;
        }
    }
}