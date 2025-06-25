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

        public GameManager(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public override void Initialize()
        {
            _signalBus.Subscribe<ChangeGameState>(GameStateChanged);
        }
        
        public override void Dispose()
        {
            _signalBus.Unsubscribe<ChangeGameState>(GameStateChanged);
        }
        
        private void GameStateChanged(ChangeGameState state)
        {
            Debug.Log(state.State);
        }
    }
}