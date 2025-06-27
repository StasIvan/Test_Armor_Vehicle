using System;
using System.Threading;
using Configs.BulletConfig;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace GameItems.BulletItem
{
    public class BulletItem : BaseGameItem, IBulletMovable, IDamageDealer
    {
        private Vector3 _direction;
        private BulletConfig _config;

        private CancellationTokenSource _cts;
        private float _duration = 5f;

        public override void Initialize()
        {
            _config = Configs.GetConfig<BulletConfigs, BulletConfig>();
        }

        public override void Dispose()
        {
            KillToken();
            Stop();
        }

        protected override void Release()
        {
            _spawner.Release(this);
        }

        public void Move()
        {
            KillToken();
            _rigidbody.AddForce(_direction * _rigidbody.mass * _config.speed, ForceMode.Impulse);
            
            _cts = new CancellationTokenSource();
            WaitToStop(_cts.Token).Forget();
        }

        public void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        public void SetDirection(Vector3 direction)
        {
            _direction = direction;
        }

        public void DealDamage(IDamageable target)
        {
            target.TakeDamage(_config.damage);
        }

        public void OnTriggerEnter(Collider other)
        {
            var dmg = other.gameObject.GetComponent<IDamageable>();
            if (dmg == null) return;
            
            DealDamage(dmg);
            Release();
        }

        private async UniTaskVoid WaitToStop(CancellationToken token)
        {
            await UniTask.WaitForSeconds(_duration, cancellationToken: token);
            Stop();
            Release();
        }
        
        private void KillToken()
        {
            if (_cts == null) return;
            _cts.Cancel();
            _cts.Dispose();   
            _cts = null;
        }
    }
}