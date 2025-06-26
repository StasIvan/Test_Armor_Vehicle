using System;
using System.Threading;
using Configs.EnemyConfig;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace GameItems.EnemyItem
{
    public class EnemyItemMove : BaseItemMove, ISeekMovement
    {
        private readonly float _patrolRadius = 2f;
        private readonly float _patrolSpeed = 1f;         
        private readonly float _delayBetweenMove = 1f;
        private readonly float _speed;
        private readonly float _rotationSpeed;
        private readonly Transform _transform;
        private readonly Vector2 _levelSize;
        private readonly Rigidbody _rigidbody;
        
        private IAnimationSetter _animationSetter;

        private CancellationTokenSource _cts;
        private Transform _target;

        private bool _isStopped;
        
        public EnemyItemMove(EnemyConfig config, Transform transform, Vector2 levelSize, Rigidbody rigidbody, IAnimationSetter animationSetter)
        {
            _speed = config.speed;
            _rotationSpeed = config.rotationSpeed;
            _transform = transform;
            _levelSize = levelSize;
            _rigidbody = rigidbody;
            _animationSetter = animationSetter;
            _isStopped = false;
        }
        
        public override void Move()
        {
            if (_isStopped) return;
            
            KillToken();
            _cts = new CancellationTokenSource();
            
            RunBehaviorAsync(_cts.Token).Forget();
        }

        public override void Stop()
        {
            if (_animationSetter != null)
                _animationSetter.SetAnimation("IsRun", false);
            
            _rigidbody.velocity = Vector3.zero;
            
            KillToken();
            _isStopped = true;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            Move();
        }

        private void KillToken()
        {
            if (_cts == null) return;
            _cts.Cancel();
            _cts.Dispose();   
            _cts = null;
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
                Vector3 offset = new Vector3(
                    UnityEngine.Random.Range(-_patrolRadius, _patrolRadius),
                    0,
                    UnityEngine.Random.Range(-_patrolRadius, _patrolRadius)
                );

                offset = new Vector3(
                    Mathf.Clamp(offset.x, -_levelSize.x, _levelSize.x),
                    0,
                    Mathf.Clamp(offset.z, -_levelSize.y, _levelSize.y)
                );

                Vector3 destination = _transform.position + offset;

                await MoveToTargetAsync(
                    () => destination,
                    _patrolSpeed,
                    () => _target == null && Vector3.Distance(_transform.position, destination) > 0.1f,
                    ct
                );

                _animationSetter.SetAnimation("IsRun", false);
                await UniTask.Delay(
                    TimeSpan.FromSeconds(UnityEngine.Random.Range(_delayBetweenMove - 0.5f, _delayBetweenMove + 0.5f)),
                    cancellationToken: ct);
            }
        }

        private async UniTask PursueAsync(CancellationToken ct)
        {
            await MoveToTargetAsync(
                () => _target.position, _speed, () => _target != null, ct);

            _animationSetter.SetAnimation("IsRun", false);
        }
        
        private async UniTask MoveToTargetAsync(
            Func<Vector3> getTargetPosition,
            float speed,
            Func<bool> continueCondition,
            CancellationToken ct)
        {
            _animationSetter.SetAnimation("IsRun", true);

            while (!ct.IsCancellationRequested)
            {
                bool shouldContinue = false;
                try
                {
                    shouldContinue = continueCondition() && _transform.gameObject != null;
                }
                catch (MissingReferenceException)
                {
                    break;
                }
                
                if (!shouldContinue)
                    break;
                
                Vector3 direction = (getTargetPosition() - _transform.position).normalized;
                Quaternion look = Quaternion.LookRotation(direction);
                _transform.rotation = Quaternion.RotateTowards(
                    _transform.rotation,
                    look,
                    _rotationSpeed * Time.fixedDeltaTime
                );

                _rigidbody.MovePosition(_transform.position + _transform.forward * (speed * Time.fixedDeltaTime));
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, ct);
            }
        }
    }
}