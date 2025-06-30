using Core.Interfaces;
using Core.Managers;
using Installers;
using UnityEngine;
using Zenject;

namespace Features.GameItems
{
    public class Billboard : MonoBehaviour
    {
        private Transform _cameraTransform;

        private Transform _transform;
        private SignalBus _signalBus;
        private bool _isCanLookAt;
        
        [Inject]
        public void Construct(IMainCameraGetter mainCamera, SignalBus signalBus)
        {
            _cameraTransform = mainCamera.GetCamera().transform;
            _signalBus = signalBus;
        }
        
        private void Awake()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<ChangeGameStateSignal>(OnGameStateChanged);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<ChangeGameStateSignal>(OnGameStateChanged);
        }

        private void LateUpdate()
        {
            if(_isCanLookAt) _transform.LookAt(_cameraTransform);
        }
        
        private void OnGameStateChanged(ChangeGameStateSignal gameState)
        {
            switch (gameState.State)
            {
                case GameState.Game:
                    _isCanLookAt = true;
                    break;
                default:
                    _isCanLookAt = false;
                    break;
            }
        }
    }
}