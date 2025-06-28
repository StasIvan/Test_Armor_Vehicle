using Base.BaseWindow;

namespace Windows.LoseGameWindow
{
    public class LoseGameController : ControllerBase<LoseGameModel, LoseGameView>
    {
        protected override LoseGameModel CreateModel()
        {
            return new LoseGameModel { IsVisible = false };
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