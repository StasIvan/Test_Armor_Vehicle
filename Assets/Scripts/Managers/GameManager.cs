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

        public GameManager(SignalBus signalBus, IStateManager stateManager)
        {
            _signalBus = signalBus;
            _stateManager = stateManager;
        }
        
        public override void Initialize()
        {
            _signalBus.Subscribe<OnContentLoadedSignal>(ContentLoaded);
            _signalBus.Subscribe<OnChangePlayerStatusSignal>(ChangePlayerStatus);
            _signalBus.Subscribe<OnPointerDownSignal>(PointerDown);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<OnContentLoadedSignal>(ContentLoaded);
            _signalBus.Unsubscribe<OnChangePlayerStatusSignal>(ChangePlayerStatus);
            _signalBus.Unsubscribe<OnPointerDownSignal>(PointerDown);
        }
        
        private void ContentLoaded()
        {
            _stateManager.ChangeState(GameState.Start);
        }
        
        private void ChangePlayerStatus(OnChangePlayerStatusSignal statusSignal)
        {
            switch (statusSignal.Status)
            {
                case PlayerStatus.Live:
                    break;
                case PlayerStatus.Dead:
                    _stateManager.ChangeState(GameState.Lose);
                    break;
                case PlayerStatus.Finished:
                    _stateManager.ChangeState(GameState.Win);
                    break;
            }
        }
        
        private void PointerDown(OnPointerDownSignal obj)
        {
            if(_stateManager.GameState != GameState.Start) return;
            _stateManager.ChangeState(GameState.Game);
        }
    }
}