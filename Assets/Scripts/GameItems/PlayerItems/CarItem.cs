using Configs.GameConfig;
using Configs.PlayerConfig;
using Installers;
using Interfaces;
using Managers;
using UnityEngine;

namespace GameItems.PlayerItems
{
    public class CarItem : BaseGameItem, IDamageable
    {
        [SerializeField] private GameItemHealth _health;
        
        private IMovable _movable;
        
        public override void Initialize()
        {
            var playerConfig = Configs.GetConfig<PlayerConfigs, PlayerConfig>();
            var gameConfig = Configs.GetConfig<GameConfigs, GameConfig>();
            _health.Initialize(playerConfig.health);
            _health.OnItemDead += OnItemDead;

            _movable = new CarItemMove(SignalBus, transform, gameConfig.levelSize, playerConfig.speed);
            
            SignalBus.Subscribe<ChangeGameStateSignal>(OnGameStateChanged);
            SignalBus.Fire(new OnChangePlayerStatusSignal() { Status = PlayerStatus.Live });
        }
        
        public override void Dispose()
        {
            _health.OnItemDead += OnItemDead;
            _health.Dispose();
            Stop();
            _movable = null;
            SignalBus.Unsubscribe<ChangeGameStateSignal>(OnGameStateChanged);
        }
        
        public void TakeDamage(float amount)
        {
            _health.TakeDamage(amount);
        }
        
        private void Move()
        {
            _movable.Move();
        }

        private void Stop()
        {
            _movable.Stop();
        }
        
        private void OnGameStateChanged(ChangeGameStateSignal gameStateSignal)
        {
            switch (gameStateSignal.State)
            {
                case GameState.Game:
                    Move();
                    break;
                default:
                    Stop();
                    break;
            }
        }
        
        protected override void Release()
        {
            _spawner.Release(this);
        }
        
        private void OnItemDead()
        {
            SignalBus.Fire(new OnChangePlayerStatusSignal() { Status = PlayerStatus.Dead });
        }
    }

    public enum PlayerStatus
    {
        Live,
        Dead,
        Finished,
    }
}