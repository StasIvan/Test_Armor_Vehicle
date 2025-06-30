using System.Threading;
using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Core.Managers.Base;
using Cysharp.Threading.Tasks;
using Features.Configs.PlayerConfig;
using Features.GameItems.Base;
using Features.GameItems.BulletItem;
using Features.GameItems.PlayerItems;
using Installers;
using UnityEngine;
using Zenject;

namespace Core.Managers
{
    public class ShootManager : BaseManager, ISettable<TurretView>
    {
        private readonly SignalBus _signalBus;
        private readonly IConfigManager _configManager;
        private readonly ISpawner _spawner;
        
        private readonly float _rotationAngle = 180f;
        private readonly float _maxDragPosition = 900f;
        
        private TurretView _turretView;
        
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
                    StopShoot();
                    break;
                case GameState.Game:
                    _signalBus.Subscribe<OnDragSignal>(OnDrag);
                    _signalBus.Subscribe<OnEndDragSignal>(OnEndDrag);
                    break;
            }
        }

        public override void Dispose()
        {
            StopShoot();
            _signalBus.TryUnsubscribe<ChangeGameStateSignal>(OnChangeGameState);
            _signalBus.TryUnsubscribe<OnDragSignal>(OnDrag);
            _signalBus.TryUnsubscribe<OnEndDragSignal>(OnEndDrag);
        }

        public void Set(TurretView value)
        {
            _turretView = value;
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
            _turretView.SetLineActive(true);
            _turretView.Rotate(deltaToApply);
            
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
            _turretView.SetLineActive(false);
            
            CancelSpawning();
        }

        private async UniTaskVoid SpawnBulletsAsync(CancellationToken ct)
        {
            float intervalSec = 60f /  GetBulletPerMinute();

            while (!ct.IsCancellationRequested)
            {
                var spawnPos = _turretView.GetSpawnBulletPosition();

                if (_spawner.GetItem(GameItemType.Bullet, spawnPos, Quaternion.identity) is BulletController item)
                    item.Move(-_turretView.transform.up);

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