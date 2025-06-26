using System;
using Configs;
using Configs.GameConfig;
using Configs.PlayerConfig;
using DG.Tweening;
using Installers;
using Interfaces;
using Managers;
using UnityEngine;
using Zenject;

namespace GameItems.PlayerItems
{
    public class CarItem : BaseGameItem, IMovable, IDamageable
    {
        private PlayerConfig _playerConfig;
        private GameConfig _gameConfig;
        
        private Tween _tween;
        private float _currentHealth;

        public override void Initialize()
        {
            _playerConfig = Configs.GetConfig<PlayerConfigs, PlayerConfig>();
            _gameConfig = Configs.GetConfig<GameConfigs, GameConfig>();
            _currentHealth = _playerConfig.health;
            SignalBus.Subscribe<ChangeGameState>(OnGameStateChanged);
        }

        public override void Dispose()
        {
            Stop();
            SignalBus.Unsubscribe<ChangeGameState>(OnGameStateChanged);
        }
        
        public void Move()
        {
            KillTween();
            Vector3 target = Vector3.forward * _gameConfig.levelSize.y;
            _tween = transform.DOMove(target, _playerConfig.speed).SetSpeedBased();
        }

        public void Stop()
        {
            KillTween();
        }

        private void KillTween()
        {
            if (_tween == null) return;
            _tween.Kill();
            _tween = null;
        }
        
        private void OnGameStateChanged(ChangeGameState gameState)
        {
            switch (gameState.State)
            {
                case GameState.Game:
                    Move();
                    break;
                case GameState.Loading:
                case GameState.Start:
                case GameState.Lose:
                case GameState.Win:
                    Stop();
                    break;
            }
        }

        public void TakeDamage(float amount)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _playerConfig.health);
            
            if(_currentHealth <= 0) 
                Stop();
        }
        
        protected override void Release()
        {
            _spawner.Release(this);
        }
    }
}