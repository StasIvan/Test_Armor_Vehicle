using System;
using Base;
using Cysharp.Threading.Tasks;
using GameItems.PlayerItems;
using Installers;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : BaseManager
    {
        private readonly SignalBus _signalBus;
        private readonly IStateManager _stateManager;
        private readonly IWindowManager _windowManager;

        public GameManager(SignalBus signalBus, IStateManager stateManager, IWindowManager windowManager)
        {
            _signalBus = signalBus;
            _stateManager = stateManager;
            _windowManager = windowManager;
        }
        
        public override void Initialize()
        {
            _signalBus.Subscribe<OnContentLoadedSignal>(ContentLoaded);
            _signalBus.Subscribe<OnChangePlayerStatusSignal>(ChangePlayerStatus);
            _signalBus.Subscribe<OnCloseWindowSignal>(CloseWindow);
        }
        

        public override void Dispose()
        {
            _signalBus.Unsubscribe<OnContentLoadedSignal>(ContentLoaded);
            _signalBus.Unsubscribe<OnChangePlayerStatusSignal>(ChangePlayerStatus);
            _signalBus.Unsubscribe<OnCloseWindowSignal>(CloseWindow);
        }
        
        private void ContentLoaded()
        {
            _windowManager.Open(WindowType.StartGameWindow);
            _stateManager.ChangeState(GameState.Start);
        }
        
        private void ChangePlayerStatus(OnChangePlayerStatusSignal statusSignal)
        {
            switch (statusSignal.Status)
            {
                case PlayerStatus.Live:
                    break;
                case PlayerStatus.Dead:
                    _windowManager.Open(WindowType.LoseGameWindow);
                    _stateManager.ChangeState(GameState.Lose);
                    break;
                case PlayerStatus.Finished:
                    _windowManager.Open(WindowType.WinGameWindow);
                    _stateManager.ChangeState(GameState.Win);
                    break;
            }
        }
        
        private void CloseWindow(OnCloseWindowSignal closeWindow)
        {
            switch (closeWindow.Type)
            {
                case WindowType.StartGameWindow:
                    _stateManager.ChangeState(GameState.Game);
                    break;
                case WindowType.LoseGameWindow:
                case WindowType.WinGameWindow:
                    _stateManager.ChangeState(GameState.Loading);
                    break;
            }
        }
        
    }
}