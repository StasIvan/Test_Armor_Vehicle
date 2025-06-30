using System;
using Core.Interfaces;
using DG.Tweening;
using Features.GameItems.Base;
using UniRx;
using UnityEngine;

namespace Features.GameItems.PlayerItems
{
    public class PlayerView : BaseItemView<PlayerModel>, IDamageable
    {
        [SerializeField] private GameItemHealth _health;
        
        private CompositeDisposable _disposables;
        private Tween _tween;
        
        private readonly Subject<Unit> _onMovementComplete = new();
        public IObservable<Unit> OnMovementComplete => _onMovementComplete;
        
        private readonly Subject<float> _onTakeDamage = new();
        public IObservable<float> OnTakeDamage => _onTakeDamage;

        public override void Initialize()
        {
            base.Initialize();
            
            _disposables = new CompositeDisposable();
            
            Model.Position.Skip(1).Subscribe(Move).AddTo(_disposables);
            
            Model.MaxHealth.Subscribe(SetMaxHealth).AddTo(_disposables);
            
            Model.Health.Subscribe(UpdateHealth).AddTo(_disposables);
            
            Model.OnResetSpeed.Subscribe(_ => KillTween()).AddTo(_disposables);
        }

        public override void Dispose()
        {
            base.Dispose();
            _disposables.Dispose();
        }

        public void TakeDamage(float amount)
        {
            _onTakeDamage.OnNext(amount);
        }
        
        private void Move(Vector3 position)
        {
            KillTween();
            _tween = transform.DOMove(position, Model.Speed.Value).SetSpeedBased();
            _tween.OnComplete(() => _onMovementComplete.OnNext(Unit.Default));
        }
        
        private void KillTween()
        {
            _tween?.Kill();
            _tween = null;
        }
        
        private void UpdateHealth(float health)
        {
            _health.SetHealth(health);
        }

        private void SetMaxHealth(float health)
        {
            _health.Initialize(health);
        }
    }
    
    public enum PlayerStatus
    {
        Live,
        Dead,
        Finished,
    }
}