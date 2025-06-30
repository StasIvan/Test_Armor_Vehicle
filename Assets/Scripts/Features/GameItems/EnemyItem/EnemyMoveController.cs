using System;
using System.Threading;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.GameItems.EnemyItem
{
    public class EnemyMoveController : ISeekMoveController, IDisposable
    {
        private readonly float _patrolRadius = 2f;
        private readonly float _patrolSpeed = 1f;
        private readonly float _delayBetweenMove = 1f;
        private readonly EnemyModel _model;
        private readonly Transform _transform;
        private readonly Vector2 _levelSize;
        private readonly string _animationName = "IsRun";
        
        private CancellationTokenSource _token;
        private Transform _target;
        
        public EnemyMoveController(EnemyModel model, Vector2 levelSize)
        {
            _model = model;
            _levelSize = levelSize;
        }

        public void Dispose()
        {
            KillToken();
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            Move();
        }

        public void Move()
        {
            KillToken();
            _token = new CancellationTokenSource();

            RunBehaviorAsync(_token.Token).Forget();
        }

        public void Stop()
        {
            _model.Animation = (_animationName, false);

            _model.ResetSpeed();

            KillToken();
        }

        private void KillToken()
        {
            if (_token == null) return;
            _token.Cancel();
            _token.Dispose();
            _token = null;
        }

        private async UniTaskVoid RunBehaviorAsync(CancellationToken ct)
        {
            await PatrolAsync(ct);

            if (_target != null)
                await PursueAsync(ct);
        }

        private async UniTask PatrolAsync(CancellationToken ct)
        {
            while (_target == null && !ct.IsCancellationRequested)
            {
                var destination = GetRandomPatrolPoint();

                await GoToPointAsync(() => destination, _patrolSpeed, ct);
                if (ct.IsCancellationRequested) return;

                _model.Animation = (_animationName, false);

                float delay = UnityEngine.Random.Range(
                    _delayBetweenMove - 0.5f, _delayBetweenMove + 0.5f);

                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: ct);
            }
        }

        private async UniTask PursueAsync(CancellationToken ct)
        {
            await GoToPointAsync(() => _target ? _target.position : _model.Position,
                _model.Speed, ct);
        }

        private async UniTask GoToPointAsync(
            Func<Vector3> targetProvider,
            float speed,
            CancellationToken ct)
        {
            _model.Animation = (_animationName, true);

            while (!ct.IsCancellationRequested)
            {
                Vector3 targetPos = targetProvider();
                if (Vector3.Distance(_model.Position, targetPos) <= 0.1f) break;

                Vector3 dir = (targetPos - _model.Position).normalized;
                Quaternion look = Quaternion.LookRotation(dir);

                _model.Rotation = Quaternion.RotateTowards(
                    _model.Rotation, look, _model.RotationSpeed * Time.fixedDeltaTime);

                _model.Position += _model.Rotation * Vector3.forward * (speed * Time.fixedDeltaTime);

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, ct);
            }

            if (!ct.IsCancellationRequested)
                _model.Animation = (_animationName, false);
        }

        private Vector3 GetRandomPatrolPoint()
        {
            float halfX = _levelSize.x * 0.5f;

            Vector3 offset = new Vector3(
                UnityEngine.Random.Range(-_patrolRadius, _patrolRadius),
                0,
                UnityEngine.Random.Range(-_patrolRadius, _patrolRadius));

            offset.x = Mathf.Clamp(offset.x, -halfX, halfX);
            offset.z = Mathf.Clamp(offset.z, 0, _levelSize.y);

            return _model.Position + offset;
        }
    }
}