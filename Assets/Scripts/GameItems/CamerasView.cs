using System;
using System.Collections.Generic;
using Cinemachine;
using Interfaces;
using UnityEngine;

namespace GameItems
{
    public class CamerasView : MonoBehaviour, IGettable<List<CameraItem>>, IMainCameraGetter
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private List<CameraItem> _cameraItems;
        
        public List<CameraItem> Get()
        {
            return _cameraItems;
        }

        public Camera GetCamera()
        {
            return _mainCamera;
        }
    }

    [Serializable]
    public struct CameraItem
    {
        public CinemachineVirtualCamera virtualCamera;
        public CameraType cameraType;
    }
    
    public enum CameraType
    {
        Start,
        Follow
    }
}