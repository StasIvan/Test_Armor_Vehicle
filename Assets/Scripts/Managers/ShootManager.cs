using System;
using System.Threading;
using Base;
using Configs.PlayerConfig;
using Cysharp.Threading.Tasks;
using GameItems;
using GameItems.BulletItem;
using GameItems.PlayerItems;
using Installers;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class ShootManager : BaseManager, ISettable<TurretItem>
    {
        private readonly SignalBus _signalBus;
        private readonly IConfigManager _configManager;
        private readonly ISpawner _spawner;
        
        private readonly float _rotationAngle = 180f;
        private readonly float _maxDragPosition = 900f;
        
        private TurretItem _turretItem;
        
        private float _currentRelativeAngle;
        
        private float _bulletPerMinute;
        private CancellationTokenSource _spawnCts;

        public ShootManager(SignalBus signalBus, IConfigManager configManager, ISpawner spawner)
        {
            _signalBus = signalBus;
            _configManager = configManager;
            _spawner = spawner;
        }
        
        public override void Initialize()
        {
            _signalBus.Subscribe<ChangeGameStateSignal>(OnChangeGameState);
        }

        private void OnChangeGameState(ChangeGameStateSignal gameState)
        {
            switch (gameState.State)
            {
                case GameState.Loading:
                case GameState.Start:
                case GameState.Lose:
                case GameState.Win:
                    _signalBus.TryUnsubscribe<OnDragSignal>(OnDrag);
                    _signalBus.TryUnsubscribe<OnEndDragSignal>(OnEndDrag);
                    break;
                case GameState.Game:
                    _signalBus.Subscribe<OnDragSignal>(OnDrag);
                    _signalBus.Subscribe<OnEndDragSignal>(OnEndDrag);
                    break;
            }
        }

        public override void Dispose()
        {
            CancelSpawning();
            _signalBus.TryUnsubscribe<ChangeGameStateSignal>(OnChangeGameState);
        }

        public void Set(TurretItem value)
        {
            _turretItem = value;
        }
        
        private void OnDrag(OnDragSignal dragSignal)
        {
            if (dragSignal.EventData.position.y > _maxDragPosition)
            {
                StopShoot();
                return;
            }
            float deltaX = -dragSignal.EventData.delta.x;
            
            var screenDistance = deltaX / Screen.width;
            
            float rotationY = screenDistance * _rotationAngle;

            float newRelative = Mathf.Clamp(_currentRelativeAngle + rotationY, -_rotationAngle / 2f, _rotationAngle / 2f);
            float deltaToApply = newRelative - _currentRelativeAngle;
            _turretItem.SetLineActive(true);
            _turretItem.Rotate(deltaToApply);
            
            _currentRelativeAngle = newRelative;
            
            if (_spawnCts == null)
            {
                _spawnCts = new CancellationTokenSource();
                SpawnBulletsAsync(_spawnCts.Token).Forget();
            }
        }
        
        private void OnEndDrag()
        {
            StopShoot();
        }

        private void StopShoot()
        {
            _turretItem.SetLineActive(false);
            
            CancelSpawning();
        }

        private async UniTaskVoid SpawnBulletsAsync(CancellationToken ct)
        {
            float intervalSec = 60f /  GetBulletPerMinute();

            while (!ct.IsCancellationRequested)
            {
                var spawnPos = _turretItem.GetSpawnBulletPosition();
                    
                var item = _spawner.GetItem<BulletItem>(spawnPos, Quaternion.identity);
                item.Initialize();
                item.SetDirection(-_turretItem.transform.up);
                item.Move();
                    
                await UniTask.WaitForSeconds(intervalSec, cancellationToken: ct);
            }
        }
        
        private void CancelSpawning()
        {
            if (_spawnCts == null) return;
            _spawnCts.Cancel();
            _spawnCts.Dispose();
            _spawnCts = null;
        }

        private float GetBulletPerMinute()
        {
            if(_bulletPerMinute <= 0f)
                _bulletPerMinute = _configManager.GetConfig<PlayerConfigs, PlayerConfig>().bulletPerMinute;
            return _bulletPerMinute;
        }
    }
}