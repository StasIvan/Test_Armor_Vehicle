using Base.BaseWindow;

namespace Windows.WinGameWindow
{
    public class WinGameController : ControllerBase<WinGameModel, WinGameView>
    {
        protected override WinGameModel CreateModel()
        {
            return new WinGameModel { IsVisible = false };
        }

        protected override void RegisterViewEvents()
        {
            view.OnCloseClicked += Close;
        }

        protected override void UnregisterViewEvents()
        {
            view.OnCloseClicked -= Close;
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