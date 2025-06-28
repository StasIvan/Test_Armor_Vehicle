using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Windows;
using Configs.WindowConfig;
using Cysharp.Threading.Tasks;
using Installers;
using Interfaces;
using Interfaces.ManagerInterfaces;
using Managers.Base;
using Zenject;

namespace Managers
{
    public class WindowsManager : BaseManager, IWindowManager
    {
        private readonly SignalBus _signalBus;
        private readonly DiContainer _container;
        private readonly Transform _parent;
        private readonly Dictionary<WindowType, GameObject> _prefabMap;
        private readonly Dictionary<WindowType, IWindowControllerFactory> _factories;
        private readonly Stack<IWindow> _stack = new Stack<IWindow>();
        private readonly Dictionary<WindowType, IWindow> _instances = new Dictionary<WindowType, IWindow>();
        private IWindow _current;
        
        public WindowsManager(UICanvas uiCanvas, SignalBus signalBus, DiContainer container,IEnumerable<IWindowControllerFactory> factories)
        {
            _signalBus = signalBus;
            _container = container;
            _parent = uiCanvas.GetWindowsTransform();
            
            _factories = factories.ToDictionary(f => f.Type);
            
            _prefabMap = new Dictionary<WindowType, GameObject>();
        }

        public override void Initialize()
        {
        }

        public override void Dispose()
        {
            ClearStack();
        }

        public async UniTask LoadWindowContent()
        {
            var configs = new WindowConfigs();
            await configs.InitAsync();
            
            foreach (var cfg in configs.GetAllConfigs())
            {
                if (!_prefabMap.ContainsKey(cfg.type))
                    _prefabMap.Add(cfg.type, cfg.prefab);
            }
        }

        public void Open(WindowType type)
        {
            if (_current?.Type == type) return;

            if (_current != null)
            {
                _current.HideWindow();
                _stack.Push(_current);
            }

            if (!_instances.TryGetValue(type, out var controller))
            {
                if (!_prefabMap.TryGetValue(type, out var prefab))
                    throw new ArgumentException($"No prefab for window type {type}");
                
                var go = _container.InstantiatePrefab(prefab, _parent);
                
                if (!_factories.TryGetValue(type, out var factory))
                    throw new InvalidOperationException($"Factory for {type} not found");

                controller = factory.Create(go, this);

                _instances[type] = controller;
            }
            
            _current = controller;

            _current.ShowWindow();
            _signalBus.Fire(new OnOpenWindowSignal { Type = type });
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