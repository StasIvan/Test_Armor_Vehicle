using Features.GameItems.Base;
using UniRx;
using UnityEngine;

namespace Features.GameItems.BulletItem
{
    public class BulletModel : BaseItemModel
    {
        public ReactiveProperty<Vector3> Direction;
        public ReactiveProperty<float> Damage;
        public ReactiveProperty<float> Speed;
        public readonly Subject<Unit> OnResetSpeed = new();


        public void ResetSpeed()
        {
            OnResetSpeed.OnNext(Unit.Default);
        }
    }
}