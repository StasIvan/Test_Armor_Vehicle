using System;
using GameItems.Base;
using UnityEngine;

namespace GameItems.PlayerItems
{
    public class PlayerModel : BaseItemModel
    {
        private Vector3 _position;
        private float _speed;
        private float _health;
        private float _maxHealth;
        public override event Action<ChangedFields> OnChanged;

        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                OnChanged?.Invoke(ChangedFields.Health);
            }
        }

        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                OnChanged?.Invoke(ChangedFields.MaxHealth);
            }
        }
        
        public float Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                OnChanged?.Invoke(ChangedFields.Speed);
            }
        }

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                OnChanged?.Invoke(ChangedFields.Position);
            }
        }
        
        public void ResetSpeed()
        {
            OnChanged?.Invoke(ChangedFields.ResetSpeed);
        }
    }
}