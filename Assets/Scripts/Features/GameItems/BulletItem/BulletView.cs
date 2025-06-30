using System;
using Core.Interfaces;
using Features.GameItems.Base;
using UnityEngine;

namespace Features.GameItems.BulletItem
{
    public class BulletView : BaseItemView<BulletModel>
    {
        private BulletModel _model;
        public event Action<IDamageable> OnDealDamage;
        public override void OnModelChanged(ChangedFields field)
        {
            if (field == ChangedFields.Position)
                UpdateImpulse();
            else if (field == ChangedFields.ResetSpeed)
                ResetSpeed();
        }

        public override void Dispose()
        {
            base.Dispose();
            OnDealDamage = null;
        }

        private void UpdateImpulse()
        {
            _rigidbody.AddForce(Model.Direction * _rigidbody.mass * Model.Speed, ForceMode.Impulse);
        }
        
        public void OnTriggerEnter(Collider other)
        {
            var dmg = other.gameObject.GetComponent<IDamageable>();
            if (dmg == null) return;
            
            OnDealDamage?.Invoke(dmg);
        }

        private void ResetSpeed()
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
}