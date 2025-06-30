using Core.Interfaces.ManagerInterfaces;
using Core.Managers.Base;
using Features.GameItems.PlayerItems;
using Installers;
using Zenject;

namespace Core.Managers
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
            _signalBus.Subscribe<OnGameItemsSpawnedSignal>(OnGameItemsSpawned);
            _signalBus.Subscribe<OnChangePlayerStatusSignal>(ChangePlayerStatus);
            _signalBus.Subscribe<ChangeGameStateSignal>(OnChangeGameState);
            _signalBus.Subscribe<OnCloseWindowSignal>(CloseWindow);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<OnGameItemsSpawnedSignal>(OnGameItemsSpawned);
            _signalBus.Unsubscribe<OnChangePlayerStatusSignal>(ChangePlayerStatus);
            _signalBus.Unsubscribe<OnCloseWindowSignal>(CloseWindow);
        }
        
        private void OnGameItemsSpawned()
        {
            _stateManager.ChangeState(GameState.Start);
        }
        
        private void ChangePlayerStatus(OnChangePlayerStatusSignal statusSignal)
        {
            switch (statusSignal.Status)
            {
                case PlayerStatus.Dead:
                    _stateManager.ChangeState(GameState.Lose);
                    break;
                case PlayerStatus.Finished:
                    _stateManager.ChangeState(GameState.Win);
                    break;
            }
        }
        
        private void OnChangeGameState(ChangeGameStateSignal gameState)
        {
            switch (gameState.State)
            {
                case GameState.Start:
                    _windowManager.Open(WindowType.StartGameWindow);
                    break;
                case GameState.Lose:
                    _windowManager.Open(WindowType.LoseGameWindow);
                    break;
                case GameState.Win:
                    _windowManager.Open(WindowType.WinGameWindow);
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