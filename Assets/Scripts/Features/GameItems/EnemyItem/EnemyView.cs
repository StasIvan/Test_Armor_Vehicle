using System;
using System.Collections.Generic;
using Core.Interfaces;
using Features.GameItems.Base;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.GameItems.EnemyItem
{
    public class EnemyView : BaseItemView<EnemyModel>, IDamageable
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private GameItemHealth _health;
        [SerializeField] private Billboard _billboard;
        private DiContainer _container;
        
        private CompositeDisposable _disposables;
        
        private readonly Subject<IDamageable> _onDealDamage = new();
        public IObservable<IDamageable> OnDealDamage => _onDealDamage;
        
        private readonly Subject<float> _onTakeDamage = new();
        public IObservable<float> OnTakeDamage => _onTakeDamage;
        
        private readonly Subject<Transform> _onFindTarget = new();
        public IObservable<Transform> OnFindTarget => _onFindTarget;
        
        [Inject]
        public void GetContainer(DiContainer container)
        {
            _container = container;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _disposables = new CompositeDisposable();
            
            _container.Inject(_billboard);
            
            Model.Position.Subscribe(UpdatePosition).AddTo(_disposables);
            
            Model.Rotation.Subscribe(UpdateRotation).AddTo(_disposables);
            
            Model.Animation.Skip(1).Subscribe(SetAnimation).AddTo(_disposables);
            
            Model.MaxHealth.Subscribe(SetMaxHealth).AddTo(_disposables);
            
            Model.Health.Subscribe(UpdateHealth).AddTo(_disposables);
            
            Model.OnResetSpeed.Subscribe(_ => ResetSpeed()).AddTo(_disposables);
        }

        public override void Dispose()
        {
            base.Dispose();
            _disposables.Dispose();
        }
        
        public void SetTarget(Transform target)
        {
            _onFindTarget.OnNext(target);
        }
        
        public void TakeDamage(float amount)
        {
            _onTakeDamage.OnNext(amount);
        }

        private void UpdateRotation(Quaternion rotation)
        {
            _transform.rotation = rotation;
        }

        private void UpdatePosition(Vector3 position)
        {
            _rigidbody.MovePosition(position);
        }

        private void SetAnimation((string name, bool value) animate)
        {
            _animator.SetBool(animate.Item1, animate.Item2);
        }

        private void UpdateHealth(float health)
        {
            _health.SetHealth(health);
        }

        private void SetMaxHealth(float health)
        {
            _health.Initialize(health);
        }

        private void ResetSpeed()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        private void OnCollisionEnter(Collision other)
        {
            var dmg = other.gameObject.GetComponent<IDamageable>();
            if (dmg == null) return;

            _onDealDamage.OnNext(dmg);
        }
    }
}