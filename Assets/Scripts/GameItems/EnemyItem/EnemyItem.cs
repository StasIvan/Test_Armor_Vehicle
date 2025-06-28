using Configs.EnemyConfig;
using Configs.GameConfig;
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
        
        [SerializeField] private GameItemHealth _health;
        [SerializeField] private Billboard _billboard;

        private EnemyItemMove _itemMove;
        
        private float _damage;
        private DiContainer _container;

        [Inject]
        public void GetContainer(DiContainer container)
        {
            _container = container;
        }
        
        public override void Initialize()
        {
            var enemyConfig = Configs.GetConfig<EnemyConfigs, EnemyConfig>();
            var gameConfig = Configs.GetConfig<GameConfigs, GameConfig>();
            _health.Initialize(enemyConfig.health);

            _container.Inject(_billboard);
            
            _damage = enemyConfig.damage;
            
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
            _health.Dispose();
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
                default:
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