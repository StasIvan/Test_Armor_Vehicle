using System;
using System.Collections.Generic;
using GameItems.Base;
using Interfaces;
using UnityEngine;
using Zenject;
using static GameItems.Base.ChangedFields;

namespace GameItems.EnemyItem
{
    public class EnemyView : BaseItemView<EnemyModel>, IDamageable
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private GameItemHealth _health;
        [SerializeField] private Billboard _billboard;
        private DiContainer _container;

        private readonly Dictionary<ChangedFields, Action> _updateActions = new();
        public event Action<IDamageable> OnDealDamage;
        public event Action<float> OnTakeDamage;
        public event Action<Transform> OnFindTarget;

        [Inject]
        public void GetContainer(DiContainer container)
        {
            _container = container;
        }

        public override void Initialize()
        {
            base.Initialize();
            _container.Inject(_billboard);

            _updateActions.Add(Position, UpdatePosition);
            _updateActions.Add(Rotation, UpdateRotation);
            _updateActions.Add(Animate, SetAnimation);
            _updateActions.Add(Health, UpdateHealth);
            _updateActions.Add(MaxHealth, SetMaxHealth);
            _updateActions.Add(ResetSpeed, Reset);
        }

        public override void Dispose()
        {
            base.Dispose();
            
            OnDealDamage = null;
            OnTakeDamage = null;
            OnFindTarget = null;
            _updateActions.Clear();
        }

        public override void Bind(EnemyModel model)
        {
            base.Bind(model);
            
            SetMaxHealth();
        }
        
        public override void OnModelChanged(ChangedFields field)
        {
            if (!_updateActions.TryGetValue(field, out var action)) throw new TypeAccessException();

            action.Invoke();
        }

        public void SetTarget(Transform target)
        {
            OnFindTarget?.Invoke(target);
        }
        
        public void TakeDamage(float amount)
        {
            OnTakeDamage?.Invoke(amount);
        }

        private void UpdateRotation()
        {
            _transform.rotation = Model.Rotation;
        }

        private void UpdatePosition()
        {
            _rigidbody.MovePosition(Model.Position);
        }

        private void SetAnimation()
        {
            _animator.SetBool(Model.Animation.Item1, Model.Animation.Item2);
        }

        private void UpdateHealth()
        {
            _health.SetHealth(Model.Health);
        }

        private void SetMaxHealth()
        {
            _health.Initialize(Model.MaxHealth);
        }

        private void Reset()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        private void OnCollisionEnter(Collision other)
        {
            var dmg = other.gameObject.GetComponent<IDamageable>();
            if (dmg == null) return;

            OnDealDamage?.Invoke(dmg);
        }
    }
}