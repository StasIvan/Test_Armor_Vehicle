using System;
using Interfaces;
using UnityEngine;

namespace GameItems
{
    public class GameItemHealth :  IDamageable
    {
        public event Action OnItemDead;
        
        private float _currentHealth;
        private readonly float _maxHealth;

        public GameItemHealth(float health)
        {
            _maxHealth = health;
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float amount)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _maxHealth);
            
            if(_currentHealth <= 0)
                OnItemDead?.Invoke();
        }
    }
}