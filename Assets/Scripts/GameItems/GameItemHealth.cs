using System;
using Configs.PlayerConfig;
using Interfaces;
using UnityEngine;
using Zenject;

namespace GameItems
{
    public class GameItemHealth : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        public event Action OnItemDead;
        
        private float _currentHealth;
        private float _maxHealth;
        private Vector3 _originalScale;
        private IConfigManager _configs;
        
        private void Awake()
        {
            _originalScale = _spriteRenderer.transform.localScale;
        }


        public void TakeDamage(float amount)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _maxHealth);
            
            float healthPercentage = _currentHealth / _maxHealth;
            _spriteRenderer.transform.localScale =
                new Vector3(_originalScale.x * healthPercentage, _originalScale.y, _originalScale.z);
            
            if(_currentHealth <= 0)
                OnItemDead?.Invoke();
        }

        public void Initialize(float health)
        {
            _spriteRenderer.transform.localScale = _originalScale;
            _maxHealth = health;
            _currentHealth = _maxHealth;
        }

        public  void Dispose()
        {
            OnItemDead = null;
        }
    }
}