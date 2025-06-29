using System;
using GameItems.Base;
using UnityEngine;

namespace GameItems.EnemyItem
{
    public class EnemyModel : BaseItemModel
    {
        public override event Action<ChangedFields> OnChanged;

        private Vector3 _position;
        private Quaternion _rotation;
        private (string, bool) _animation;
        private float _health;
        private float _maxHealth;

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

        public float Speed { get; set; }
        
        public float RotationSpeed { get; set; }
        
        public float Damage { get; set; }

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                OnChanged?.Invoke(ChangedFields.Position);
            }
        }

        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                OnChanged?.Invoke(ChangedFields.Rotation);
            }
        }

        public (string, bool) Animation
        {
            get => _animation;
            set
            {
                _animation = value;
                OnChanged?.Invoke(ChangedFields.Animate);
            }
        }

        public void ResetSpeed()
        {
            OnChanged?.Invoke(ChangedFields.ResetSpeed);
        }
        
    }
}