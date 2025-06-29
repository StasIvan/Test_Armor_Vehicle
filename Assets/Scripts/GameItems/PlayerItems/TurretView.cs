using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace GameItems.PlayerItems
{
    public class TurretView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _line;
        [SerializeField] private Transform _bulletSpawnPoint;
        private Transform _transform;
        
        private void Awake()
        {
            _transform = transform;
            SetLineActive(false);
        }

        private void OnEnable()
        {
            SetLineActive(false);
        }

        [Inject]
        public void Construct(ISettable<TurretView> settable)
        {
            settable.Set(this);
        }
        
        public void Rotate(float rotationY)
        {
            _transform.Rotate(0f, rotationY, 0f, Space.World);
        }

        public void SetLineActive(bool active)
        {
            _line.enabled = active;
        }

        public Vector3 GetSpawnBulletPosition()
        {
            return _bulletSpawnPoint.position;
        }
    }
}