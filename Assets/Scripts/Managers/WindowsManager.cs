using System.Collections.Generic;
using UnityEngine;
using System;
using Windows;
using Base;
using Configs.WindowConfig;
using Cysharp.Threading.Tasks;
using Installers;
using Interfaces;
using Zenject;
using Object = UnityEngine.Object;

namespace Managers
{
    public class WindowsManager : BaseManager, IWindowManager
    {
        private readonly SignalBus _signalBus;
        private readonly DiContainer _container;
        private readonly Transform _parent;
        private readonly Dictionary<WindowType, GameObject> _prefabMap;

        private readonly Stack<IWindow> _stack = new Stack<IWindow>();
        private readonly Dictionary<WindowType, IWindow> _instances = new Dictionary<WindowType, IWindow>();
        private IWindow _current;
        
        public WindowsManager(UICanvas uiCanvas, SignalBus signalBus, DiContainer container)
        {
            _signalBus = signalBus;
            _container = container;
            _parent = uiCanvas.GetWindowsTransform();
            _prefabMap = new Dictionary<WindowType, GameObject>();
        }

        public override void Initialize()
        {
            LoadConfigs().Forget();
        }

        private async UniTask LoadConfigs()
        {
            var configs = new WindowConfigs();
            await configs.InitAsync();
            
            foreach (var cfg in configs.GetAllConfigs())
            {
                if (!_prefabMap.ContainsKey(cfg.type))
                    _prefabMap.Add(cfg.type, cfg.prefab);
            }
        }
        
        public override void Dispose()
        {
            ClearStack();
        }

        public void Open(WindowType type)
        {
            if (_current != null && _current.Type == type)
                return;

            if (_current != null)
            {
                _current.HideWindow();
                _stack.Push(_current);
            }

            if (!_instances.TryGetValue(type, out var window))
            {
                if (!_prefabMap.TryGetValue(type, out var prefab))
                    throw new ArgumentException($"No prefab for window type {type}");

                var go = _container.InstantiatePrefab(prefab, _parent);
                
                if (!(go.TryGetComponent<IWindow>(out window)))
                    throw new InvalidOperationException($"Window prefab {prefab.name} must implement IWindow");

                _instances[type] = window;
            }

            _current = window;
            _current.ShowWindow();
            _signalBus.Fire(new OnOpenWindowSignal() {Type = type});
        }

        public void Close()
        {
            if (_current == null)
            {
                _signalBus.Fire<OnCloseAllWindowSignal>();
                return;
            }

            var closingType = _current.Type;
            _current.HideWindow();
            _signalBus.Fire(new OnCloseWindowSignal() {Type = closingType});
            
            if (_stack.Count > 0)
            {
                _current = _stack.Pop();
                _current.ShowWindow();
                _signalBus.Fire(new OnOpenWindowSignal() {Type = _current.Type});
            }
            else
            {
                _current = null;
                _signalBus.Fire<OnCloseAllWindowSignal>();
            }
        }

        private void ClearStack()
        {
            _stack.Clear();  
        } 
    }

    public enum WindowType
    {
        None,
        StartGameWindow,
        LoseGameWindow,
        WinGameWindow,
    }
}