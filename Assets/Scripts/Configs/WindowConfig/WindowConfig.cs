using System;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs.WindowConfig
{
    [Serializable]
    public class WindowConfig : BaseConfig
    {
        public WindowType type;
        public GameObject prefab;
    }
}