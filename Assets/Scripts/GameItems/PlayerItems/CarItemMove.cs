using DG.Tweening;
using Installers;
using UnityEngine;
using Zenject;

namespace GameItems.PlayerItems
{
    public class CarItemMove : BaseItemMove
    {
        private readonly SignalBus _signalBus;
        private readonly Transform _transform;
        private readonly Vector2 _levelSize;
        private readonly float _speed;
        private Tween _tween;

        public CarItemMove(SignalBus signalBus, Transform transform, Vector2 levelSize, float speed)
        {
            _signalBus = signalBus;
            _transform = transform;
            _levelSize = levelSize;
            _speed = speed;
        }
        
        public override void Move()
        {
            KillTween();
            Vector3 target = Vector3.forward * _levelSize.y;
            _tween = _transform.DOMove(target, _speed).SetSpeedBased();
            _tween.OnComplete(() => _signalBus.Fire(new OnChangePlayerStatusSignal() { Status = PlayerStatus.Finished }));
        }

        public override void Stop()
        {
            KillTween();
        }
        
        private void KillTween()
        {
            if (_tween == null) return;
            _tween.Kill();
            _tween = null;
        }
    }
}