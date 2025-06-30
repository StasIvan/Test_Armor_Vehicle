﻿using Core.Interfaces;
using UnityEngine;

namespace Features.Configs.WindowConfig
{
    [CreateAssetMenu(fileName = "Window", menuName = "Configs/WindowConfig", order = 4)]
    public class WindowConfigSO : ScriptableObject, IConfigContainer<WindowConfig>
    {
        [SerializeField] private WindowConfig _config;

        public WindowConfig GetConfig()
        {
            return _config;
        }
    }
}