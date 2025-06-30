using System;
using Core.Interfaces;
using Core.Managers;
using Features.Configs.GameConfig;
using Features.GameItems.Base;
using Installers;
using UniRx;
using UnityEngine;

namespace Features.GameItems.EnemyItem
{
    public class EnemyController : BaseItemController<EnemyView, EnemyModel>
    {
        private ISeekMoveController _moveController;
        public override GameItemType Type { get => GameItemType.Enemy; }
        
        public override void Initialize()
        {
            Disposables = new CompositeDisposable();
            SignalBus.Subscribe<ChangeGameStateSignal>(ChangeGameState);
            
            View.OnDealDamage.Subscribe(OnDealDamage).AddTo(Disposables);
            View.OnFindTarget.Subscribe(SetTarget).AddTo(Disposables);
            View.OnTakeDamage.Subscribe(TakeDamage).AddTo(Disposables);
        }
        
        public override void Dispose()
        {
            SignalBus.Unsubscribe<ChangeGameStateSignal>(ChangeGameState);
            Disposables.Dispose();
            ((IDisposable)_moveController)?.Dispose();
        }

        public override void Bind(EnemyView view, EnemyModel model)
        {
            View = view;
            Model = model;

            CreateMoveController();
        }

        private void CreateMoveController()
        {
            if (_moveController != null) DisposeController(_moveController as IDisposable);

            _moveController =
                new EnemyMoveController(Model, ConfigManager.GetConfig<GameConfigs, GameConfig>().levelSize);
        }

        private void ChangeGameState(ChangeGameStateSignal gameState)
        {
            switch (gameState.State)
            {
                case GameState.Game:
                    _moveController.Move();
                    break;
                default:
                    _moveController.Stop();
                    break;
            }
        }

        private void SetTarget(Transform target)
        {
            _moveController.SetTarget(target);
        }
        
        private void TakeDamage(float amount)
        {
            Model.Health.Value = Mathf.Clamp(Model.Health.Value - amount, 0, Model.MaxHealth.Value);

            if (Model.Health.Value <= 0)
                Release();
        }
        
        private void OnDealDamage(IDamageable target)
        {
            target.TakeDamage(Model.Damage.Value);
            Release();
        }
        
        private void DisposeController(IDisposable controller)
        {
            controller.Dispose();
        }
        
        private void Release()
        {
            Spawner.Release(this);
        }
    }
}