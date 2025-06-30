using System;
using Features.GameItems.Base;
using UniRx;
using UnityEngine;

namespace Features.GameItems.PlayerItems
{
    public class PlayerModel : BaseItemModel
    {
        public ReactiveProperty<Vector3> Position;
        public ReactiveProperty<float> Health;
        public ReactiveProperty<float> MaxHealth;
        public ReactiveProperty<float> Speed;
        public readonly Subject<Unit> OnResetSpeed = new();
        
        public void ResetSpeed()
        {
            OnResetSpeed.OnNext(Unit.Default);
        }
    }
}