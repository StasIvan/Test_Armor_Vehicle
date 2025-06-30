using System.Threading;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Features.GameItems.Base;
using UnityEngine;

namespace Features.GameItems.BulletItem
{
    public class BulletController : BaseItemController<BulletView, BulletModel>
    {
        public override GameItemType Type { get => GameItemType.Bullet; }
        
        private CancellationTokenSource _token;
        private readonly float _duration = 5f;

        public override void Initialize()
        {
        }

        public override void Dispose()
        {
            KillToken();
            Stop();
        }
        
        public override void Bind(BulletView view, BulletModel model)
        {
            View = view;
            Model = model;
            view.OnDealDamage += DealDamage;
        }
        
        public void Move(Vector3 direction)
        {
            KillToken();
            
            Model.Direction = direction;
            
            _token = new CancellationTokenSource();
            
            WaitToStop(_token.Token).Forget();
        }

        private void Stop()
        {
            Model.ResetSpeed();
        }

        private void DealDamage(IDamageable target)
        {
            target.TakeDamage(Model.Damage);
            Release();
        }
        
        private async UniTaskVoid WaitToStop(CancellationToken token)
        {
            await UniTask.WaitForSeconds(_duration, cancellationToken: token);
            Release();
        }
        
        private void KillToken()
        {
            if (_token == null) return;
            _token.Cancel();
            _token.Dispose();   
            _token = null;
        }

        private void Release()
        {
            Stop();
            Spawner.Release(this);
        }
    }
}