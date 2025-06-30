using System;
using System.Collections.Generic;
using Core.Interfaces;
using DG.Tweening;
using Features.GameItems.Base;
using UnityEngine;
using static Features.GameItems.Base.ChangedFields;

namespace Features.GameItems.PlayerItems
{
    public class PlayerView : BaseItemView<PlayerModel>, IDamageable
    {
        [SerializeField] private GameItemHealth _health;
        
        public event Action OnMovementComplete;
        public event Action<float> OnTakeDamage;
        private Tween _tween;
        private readonly Dictionary<ChangedFields,Action> _updateActions = new();

        public override void Initialize()
        {
            base.Initialize();

            _updateActions.Add(Position, Move);
            _updateActions.Add(Health, UpdateHealth);
            _updateActions.Add(MaxHealth, SetMaxHealth);
            _updateActions.Add(ResetSpeed, KillTween);
        }

        public override void Dispose()
        {
            base.Dispose();
            OnMovementComplete = null;
            OnTakeDamage = null;
            _updateActions.Clear();
        }

        public override void Bind(PlayerModel model)
        {
            base.Bind(model);
            
            SetMaxHealth();
        }

        public override void OnModelChanged(ChangedFields field)
        {
            if (!_updateActions.TryGetValue(field, out var action)) throw new TypeAccessException();

            action.Invoke();
        }

        public void TakeDamage(float amount)
        {
            OnTakeDamage?.Invoke(amount);
        }
        
        private void Move()
        {
            KillTween();
            _tween = transform.DOMove(Model.Position, Model.Speed).SetSpeedBased();
            _tween.OnComplete(() => OnMovementComplete?.Invoke());
        }
        
        private void KillTween()
        {
            _tween?.Kill();
            _tween = null;
        }
        
        private void UpdateHealth()
        {
            _health.SetHealth(Model.Health);
        }

        private void SetMaxHealth()
        {
            _health.Initialize(Model.MaxHealth);
        }
    }
    
    public enum PlayerStatus
    {
        Live,
        Dead,
        Finished,
    }
}