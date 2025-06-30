using Core.Managers;
using Features.Configs.GameConfig;
using Features.GameItems.Base;
using Installers;
using UniRx;
using UnityEngine;

namespace Features.GameItems.PlayerItems
{
    public class PlayerController : BaseItemController<PlayerView, PlayerModel>
    {
        public override GameItemType Type { get => GameItemType.Player; }
        private Vector2 _levelSize;

        public override void Initialize()
        {
            Disposables = new CompositeDisposable();
            SignalBus.Subscribe<ChangeGameStateSignal>(OnGameStateChanged);
            _levelSize = ConfigManager.GetConfig<GameConfigs, GameConfig>().levelSize;
            
            View.OnTakeDamage.Subscribe(TakeDamage).AddTo(Disposables);
            View.OnMovementComplete.Subscribe(_ => OnMovementFinished()).AddTo(Disposables);
        }
        
        public override void Dispose()
        {
            SignalBus.Unsubscribe<ChangeGameStateSignal>(OnGameStateChanged);
            Disposables.Dispose();
        }

        public override void Bind(PlayerView view, PlayerModel model)
        {
            View = view;
            Model = model;
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
            Model.Position.Value = target;
        }
        
        private void TakeDamage(float amount)
        {
            Model.Health.Value = Mathf.Clamp(Model.Health.Value - amount, 0, Model.MaxHealth.Value);

            if (Model.Health.Value <= 0)
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