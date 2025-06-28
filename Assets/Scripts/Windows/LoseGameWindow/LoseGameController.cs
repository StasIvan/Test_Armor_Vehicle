using Windows.BaseWindow;
using Base.BaseWindow;
using Interfaces;
using Interfaces.ManagerInterfaces;
using Managers;

namespace Windows.LoseGameWindow
{
    public class LoseGameController : ControllerBase<LoseGameModel, LoseGameView>
    {
        public LoseGameController(WindowType type, LoseGameModel model, LoseGameView view, IWindowManager windowManager) : base(type, model, view, windowManager)
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