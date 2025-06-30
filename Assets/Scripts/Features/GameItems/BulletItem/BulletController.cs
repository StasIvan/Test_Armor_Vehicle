using System.Threading;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Features.GameItems.Base;
using UniRx;
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
            Disposables = new CompositeDisposable();
            View.OnDealDamage.Subscribe(DealDamage).AddTo(Disposables);
        }

        public override void Dispose()
        {
            KillToken();
            Disposables.Dispose();
            Stop();
        }
        
        public override void Bind(BulletView view, BulletModel model)
        {
            View = view;
            Model = model;
        }
        
        public void Move(Vector3 direction)
        {
            KillToken();
            //Debug.Log("HashCode = "+ Model.GetHashCode() + " Move = " + direction);
            Model.Direction.Value = direction;
            
            _token = new CancellationTokenSource();
            
            WaitToStop(_token.Token).Forget();
        }

        private void Stop()
        {
            Model.ResetSpeed();
        }

        private void DealDamage(IDamageable target)
        {
            target.TakeDamage(Model.Damage.Value);
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