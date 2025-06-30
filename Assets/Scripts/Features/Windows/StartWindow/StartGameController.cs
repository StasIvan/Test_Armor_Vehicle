using Core.Interfaces.ManagerInterfaces;
using Core.Managers;
using Features.Windows.BaseWindow;

namespace Features.Windows.StartWindow
{
    public class StartGameController: ControllerBase<StartGameModel, StartGameView>
    {
        public StartGameController(WindowType type, StartGameModel model, StartGameView view, IWindowManager windowManager) : base(type, model, view, windowManager)
        {
        }

        protected override void RegisterViewEvents()
        {
            View.OnCloseClicked += Close;
        }

        protected override void UnregisterViewEvents()
        {
            View.OnCloseClicked -= Close;
        }
        
        protected override void OnShow()
        {
            Model.IsVisible = true;
        }

        protected override void OnHide()
        {
            Model.IsVisible = false;
        }
        
    }
}