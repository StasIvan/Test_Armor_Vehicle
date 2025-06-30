using System;
using Features.GameItems.Base;
using UniRx;
using UnityEngine;

namespace Features.GameItems.EnemyItem
{
    public class EnemyModel : BaseItemModel
    {
        public ReactiveProperty<Vector3> Position;
        public ReactiveProperty<float> Health;
        public ReactiveProperty<float> MaxHealth;
        public ReactiveProperty<float> Speed;
        public ReactiveProperty<float> RotationSpeed;
        public ReactiveProperty<float> Damage;
        public ReactiveProperty<Quaternion> Rotation;
        public readonly ReactiveProperty<(string, bool)> Animation = new();
        public readonly Subject<Unit> OnResetSpeed = new();
        
        public void ResetSpeed()
        {
            OnResetSpeed.OnNext(Unit.Default);
        }
        
    }
}