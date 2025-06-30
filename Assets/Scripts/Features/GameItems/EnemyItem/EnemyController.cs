using System;
using Core.Interfaces;
using Core.Managers;
using Features.Configs.GameConfig;
using Features.GameItems.Base;
using Installers;
using UnityEngine;

namespace Features.GameItems.EnemyItem
{
    public class EnemyController : BaseItemController<EnemyView, EnemyModel>
    {
        private ISeekMoveController _moveController;
        public override GameItemType Type { get => GameItemType.Enemy; }
        public override void Initialize()
        {
            SignalBus.Subscribe<ChangeGameStateSignal>(ChangeGameState);
        }
        
        public override void Dispose()
        {
            SignalBus.Unsubscribe<ChangeGameStateSignal>(ChangeGameState);
            
            ((IDisposable)_moveController)?.Dispose();
        }

        public override void Bind(EnemyView view, EnemyModel model)
        {
            View = view;
            Model = model;

            CreateMoveController();

            View.OnDealDamage += OnDealDamage;
            View.OnFindTarget += SetTarget;
            View.OnTakeDamage += TakeDamage;
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
            Model.Health = Mathf.Clamp(Model.Health - amount, 0, Model.MaxHealth);

            if (Model.Health <= 0)
                Release();
        }
        
        private void OnDealDamage(IDamageable target)
        {
            target.TakeDamage(Model.Damage);
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