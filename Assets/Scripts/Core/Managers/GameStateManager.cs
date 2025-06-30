using Core.Interfaces.ManagerInterfaces;
using Core.Managers.Base;
using Installers;
using Zenject;

namespace Core.Managers
{
    public class GameStateManager : BaseManager, IStateManager
    {
        public GameState GameState { get; private set; }
        
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
            GameState = state;
            _signalBus.Fire(new ChangeGameStateSignal() { State = GameState });
        }
    }

    public enum GameState
    {
        None,
        Loading,
        Start,
        Game,
        Lose,
        Win
    }
}