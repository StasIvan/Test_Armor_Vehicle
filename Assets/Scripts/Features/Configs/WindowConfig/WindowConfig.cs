using System;
using Core.Managers;
using UnityEngine;

namespace Features.Configs.WindowConfig
{
    [Serializable]
    public class WindowConfig : BaseConfig
    {
        public WindowType type;
        public GameObject prefab;
    }
}