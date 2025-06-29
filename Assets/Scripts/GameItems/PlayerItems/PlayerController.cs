using Configs.GameConfig;
using GameItems.Base;
using Installers;
using Managers;
using UnityEngine;

namespace GameItems.PlayerItems
{
    public class PlayerController : BaseItemController<PlayerView, PlayerModel>
    {
        public override GameItemType Type { get => GameItemType.Player; }
        private Vector2 _levelSize;

        public override void Initialize()
        {
            SignalBus.Subscribe<ChangeGameStateSignal>(OnGameStateChanged);
            _levelSize = ConfigManager.GetConfig<GameConfigs, GameConfig>().levelSize;
        }
        
        public override void Dispose()
        {
            SignalBus.Unsubscribe<ChangeGameStateSignal>(OnGameStateChanged);
            
            //Release();
        }

        public override void Bind(PlayerView view, PlayerModel model)
        {
            View = view;
            Model = model;

            View.OnTakeDamage += TakeDamage;
            View.OnMovementComplete += OnMovementFinished;
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
        
        private void Move()
        {
            Vector3 target = Vector3.forward * _levelSize.y;
            Model.Position = target;
        }
        
        private void TakeDamage(float amount)
        {
            Model.Health = Mathf.Clamp(Model.Health - amount, 0, Model.MaxHealth);

            if (Model.Health <= 0)
                OnItemDead();
        }

        private void Stop()
        {
            Model.ResetSpeed();
        }
        
        private void OnMovementFinished()
        {
            SignalBus.Fire(new OnChangePlayerStatusSignal { Status = PlayerStatus.Finished });
        }
        
        private void OnItemDead()
        {
            SignalBus.Fire(new OnChangePlayerStatusSignal() { Status = PlayerStatus.Dead });
        }
        
        private void Release()
        {
            Spawner.Release(this);
        }
        
    }
}