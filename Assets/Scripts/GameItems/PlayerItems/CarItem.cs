using Configs.GameConfig;
using Configs.PlayerConfig;
using Installers;
using Interfaces;
using Managers;

namespace GameItems.PlayerItems
{
    public class CarItem : BaseGameItem, IDamageable
    {
        private GameItemHealth _health;
        
        private IMovable _movable;
        
        public override void Initialize()
        {
            var playerConfig = Configs.GetConfig<PlayerConfigs, PlayerConfig>();
            var gameConfig = Configs.GetConfig<GameConfigs, GameConfig>();
            _health = new GameItemHealth(playerConfig.health);
            _health.OnItemDead += OnItemDead;

            _movable = new CarItemMove(SignalBus, transform, gameConfig.levelSize, playerConfig.speed);
            
            SignalBus.Subscribe<ChangeGameStateSignal>(OnGameStateChanged);
            SignalBus.Fire(new OnChangePlayerStatusSignal() { Status = PlayerStatus.Live });
        }
        
        public override void Dispose()
        {
            _health.OnItemDead += OnItemDead;
            _health = null;
            Stop();
            _movable = null;
            SignalBus.Unsubscribe<ChangeGameStateSignal>(OnGameStateChanged);
        }
        
        private void Move()
        {
            _movable.Move();
        }

        private void Stop()
        {
            _movable.Stop();
        }

        public void TakeDamage(float amount)
        {
            _health.TakeDamage(amount);
        }
        
        private void OnGameStateChanged(ChangeGameStateSignal gameStateSignal)
        {
            switch (gameStateSignal.State)
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