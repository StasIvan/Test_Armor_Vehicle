using Base;
using Cysharp.Threading.Tasks;
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
            _signalBus.Subscribe<ChangeGameState>(GameStateChanged);
            _signalBus.Subscribe<OnPointerDown>(PointerDown);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<ChangeGameState>(GameStateChanged);
            _signalBus.Unsubscribe<OnPointerDown>(PointerDown);
        }
        
        private void GameStateChanged(ChangeGameState state)
        {
           
        }
        
        private void PointerDown(OnPointerDown obj)
        {
            if(_stateManager.GameState != GameState.Start) return;
            _stateManager.ChangeState(GameState.Game);
        }
    }
}