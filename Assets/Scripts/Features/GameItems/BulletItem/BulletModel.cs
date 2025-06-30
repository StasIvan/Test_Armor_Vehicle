using System;
using Features.GameItems.Base;
using UnityEngine;

namespace Features.GameItems.BulletItem
{
    public class BulletModel : BaseItemModel
    {
        public override event Action<ChangedFields> OnChanged;

        private Vector3 _direction;

        public Vector3 Direction
        {
            get => _direction;
            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    OnChanged?.Invoke(ChangedFields.Position);
                }
            }
        }

        public float Damage { get; set; }

        public float Speed { get; set; }

        public void ResetSpeed()
        {
            OnChanged?.Invoke(ChangedFields.ResetSpeed);
        }
    }
}