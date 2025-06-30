using System;
using Core.Interfaces;
using Features.GameItems.Base;
using UniRx;
using UnityEngine;

namespace Features.GameItems.BulletItem
{
    public class BulletView : BaseItemView<BulletModel>
    {
        private readonly Subject<IDamageable> _onDealDamage = new();
        public IObservable<IDamageable> OnDealDamage => _onDealDamage;
        private CompositeDisposable _disposables;

        public override void Initialize()
        {
            base.Initialize();
            _disposables = new CompositeDisposable();
            Model.OnResetSpeed.Subscribe(_ => ResetSpeed()).AddTo(_disposables);
            Model.Direction.Subscribe(UpdateImpulse).AddTo(_disposables);
        }

        public override void Dispose()
        {
            base.Dispose();
            _disposables.Dispose();
        }

        private void UpdateImpulse(Vector3 impulse)
        {
            _rigidbody.AddForce(impulse * _rigidbody.mass * Model.Speed.Value, ForceMode.Impulse);
        }
        
        public void OnTriggerEnter(Collider other)
        {
            var dmg = other.gameObject.GetComponent<IDamageable>();
            if (dmg == null) return;
            
            _onDealDamage.OnNext(dmg);
        }

        private void ResetSpeed()
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
}