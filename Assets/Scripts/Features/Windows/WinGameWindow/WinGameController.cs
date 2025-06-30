using Core.Interfaces.ManagerInterfaces;
using Core.Managers;
using Features.Windows.BaseWindow;

namespace Features.Windows.WinGameWindow
{
    public class WinGameController : ControllerBase<WinGameModel, WinGameView>
    {
        public WinGameController(WindowType type, WinGameModel model, WinGameView view, IWindowManager windowManager) : base(type, model, view, windowManager)
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