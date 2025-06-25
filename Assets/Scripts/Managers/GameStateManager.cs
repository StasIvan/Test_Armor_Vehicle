using Base;
using Installers;
using Interfaces;
using Zenject;

namespace Managers
{
    public class GameStateManager : BaseManager, IStateManager
    {
        private readonly SignalBus _signalBus;

        public GameStateManager(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public override void Initialize()
        {
            
        }

        public override void Dispose()
        {
        }

        public void ChangeState(GameState state)
        {
            _signalBus.Fire(new ChangeGameState() { State = state });
        }
    }

    public enum GameState
    {
        None,
        Loading,
        Start
    }
}