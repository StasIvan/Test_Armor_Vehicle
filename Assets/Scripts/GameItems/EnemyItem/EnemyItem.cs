using System;
using System.Threading;
using Configs;
using Configs.EnemyConfig;
using Configs.GameConfig;
using Cysharp.Threading.Tasks;
using Installers;
using Interfaces;
using Managers;
using UnityEngine;
using Zenject;

namespace GameItems.EnemyItem
{
    public class EnemyItem : BaseGameItem, IDamageDealer, IDamageable
    {
        [SerializeField] private Animator _animator;
        
        private GameItemHealth _health;

        private EnemyItemMove _itemMove;
        
        private float _damage;

        public override void Initialize()
        {
            var enemyConfig = Configs.GetConfig<EnemyConfigs, EnemyConfig>();
            var gameConfig = Configs.GetConfig<GameConfigs, GameConfig>();

            _damage = enemyConfig.damage;
            
            _health = new GameItemHealth(enemyConfig.health);
            _health.OnItemDead += Release;

            _itemMove = new EnemyItemMove(enemyConfig, transform, gameConfig.levelSize, _rigidbody,
                new AnimatorController(_animator));
            
            SignalBus.Subscribe<ChangeGameStateSignal>(ChangeGameState);
        }
        
        public void SetTarget(Transform target)
        {
            _itemMove.SetTarget(target);
        }
        
        public void TakeDamage(float amount)
        {
            _health.TakeDamage(amount);
        }
        
        public override void Dispose()
        {
            _health.OnItemDead -= Release;
            _health = null;
            _itemMove.Stop();
            _itemMove = null;
            SignalBus.Unsubscribe<ChangeGameStateSignal>(ChangeGameState);
        }

        public void DealDamage(IDamageable target)
        {
            target.TakeDamage(_damage);
        }

        private void ChangeGameState(ChangeGameStateSignal gameState)
        {
            switch (gameState.State)
            {
                case GameState.Game:
                    _itemMove.Move();
                    break;
                case GameState.Lose:
                case GameState.Win:
                    _itemMove.Stop();
                    break;
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