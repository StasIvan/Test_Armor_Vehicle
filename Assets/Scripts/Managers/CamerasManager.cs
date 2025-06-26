using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using GameItems;
using GameItems.PlayerItems;
using Installers;
using Interfaces;
using Zenject;

namespace Managers
{
    public class CamerasManager : BaseManager, ISettable<CarItem>
    {
        private readonly List<CameraItem> _cameraItems;
        private readonly SignalBus _signalBus;

        public CamerasManager(SignalBus signalBus, IGettable<List<CameraItem>> gettable)
        {
            _signalBus = signalBus;
            _cameraItems = gettable.Get();
        }
        
        public override void Initialize()
        {
            _signalBus.Subscribe<ChangeGameStateSignal>(GameStateChanged);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<ChangeGameStateSignal>(GameStateChanged);
        }
        
        public void Set(CarItem value)
        {
            foreach (var item in _cameraItems)
            {
                switch (item.cameraType)
                {
                    case CameraType.Start:
                        item.virtualCamera.LookAt = value.transform;
                        break;
                    case CameraType.Follow:
                        item.virtualCamera.Follow = value.transform;
                        break;
                }
            }
        }
        
        private void GameStateChanged(ChangeGameStateSignal gameStateSignal)
        {
            switch (gameStateSignal.State)
            {
                case GameState.Loading:
                    break;
                case GameState.Start:
                    SetCamera(CameraType.Start);
                    break;
                case GameState.Game:
                    SetCamera(CameraType.Follow);
                    break;
                
            }
        }

        private void SetCamera(CameraType cameraType)
        {
            DisableAllCameras();
            ActiveCamera(cameraType);
        }

        private void DisableAllCameras()
        {
            foreach (var item in _cameraItems)
            {
                item.virtualCamera.gameObject.SetActive(false);
            }
        }

        private void ActiveCamera(CameraType cameraType)
        {
            var virtualCamera = _cameraItems.FirstOrDefault(v => v.cameraType == cameraType).virtualCamera;
            virtualCamera.gameObject.SetActive(true);
        }
    }
}