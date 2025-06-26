using System;
using System.Threading;
using Configs;
using Configs.EnemyConfig;
using Configs.GameConfig;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using Zenject;

namespace GameItems.EnemyItem
{
    public class EnemyItem : BaseGameItem, IDamageable, IDamageDealer, ISeekMovement
    {
        [SerializeField] private Animator _animator;
        
        private readonly float _patrolRadius = 2f;
        private readonly float _patrolSpeed = 1f;         
        private readonly float _delayBetweenMove = 1f;
        
        private EnemyConfig _enemyConfig;
        private GameConfig _gameConfig;
        private CancellationTokenSource _cts;
        private Transform _target;
        
        private float _currentHealth;
        
        public override void Initialize()
        {
            _enemyConfig = Configs.GetConfig<EnemyConfigs, EnemyConfig>();
            _gameConfig = Configs.GetConfig<GameConfigs, GameConfig>();
            _currentHealth = _enemyConfig.health;
            Move();
        }

        public override void Dispose()
        {
            Stop();
        }

        public void TakeDamage(float amount)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _enemyConfig.health);

            if (_currentHealth <= 0)
                Release();
        }

        public void DealDamage(IDamageable target)
        {
            target.TakeDamage(_enemyConfig.damage);
        }

        public void Move()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();   
            }
            _cts = new CancellationTokenSource();
            
            RunBehaviorAsync(_cts.Token).Forget();
        }

        public void Stop()
        {
            _cts.Cancel();
            _cts.Dispose();   
            _cts = null;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            Move();
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
                    Mathf.Clamp(offset.x, -_gameConfig.levelSize.x, _gameConfig.levelSize.x),
                    0,
                    Mathf.Clamp(offset.z, -_gameConfig.levelSize.y, _gameConfig.levelSize.y)
                );

                Vector3 destination = transform.position + offset;

                await MoveToTargetAsync(
                    () => destination,
                    _patrolSpeed,
                    () => _target == null && Vector3.Distance(transform.position, destination) > 0.1f,
                    ct
                );

                SetIdleAnimation();
                await UniTask.Delay(
                    TimeSpan.FromSeconds(UnityEngine.Random.Range(_delayBetweenMove - 0.5f, _delayBetweenMove + 0.5f)),
                    cancellationToken: ct);
            }
        }

        private async UniTask PursueAsync(CancellationToken ct)
        {
            await MoveToTargetAsync(
                () => _target.position,
                _enemyConfig.speed,
                () => _target != null,
                ct
            );

            SetIdleAnimation();
        }
        
        private async UniTask MoveToTargetAsync(
            Func<Vector3> getTargetPosition,
            float speed,
            Func<bool> continueCondition,
            CancellationToken ct)
        {
            SetRunAnimation();

            while (!ct.IsCancellationRequested)
            {
                bool shouldContinue = false;
                try
                {
                    shouldContinue = continueCondition() && this != null && gameObject != null;
                }
                catch (MissingReferenceException)
                {
                    break;
                }
                
                if (!shouldContinue)
                    break;
                
                Vector3 direction = (getTargetPosition() - transform.position).normalized;
                Quaternion look = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    look,
                    _enemyConfig.rotationSpeed * Time.fixedDeltaTime
                );

                _rigidbody.MovePosition(transform.position + transform.forward * (speed * Time.fixedDeltaTime));
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, ct);
            }
        }
        
        private void SetIdleAnimation()
        {
            if (_animator != null)
            {
                _animator.SetBool("IsRun", false);
            }
        }

        private void SetRunAnimation()
        {
            if (_animator != null)
            {
                _animator.SetBool("IsRun", true);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var dmg = other.gameObject.GetComponent<IDamageable>();
            if (dmg == null) return;
            
            DealDamage(dmg);
            Release();
        }

        protected override void Release()
        {
            _spawner.Release(this);
        }
    }
}