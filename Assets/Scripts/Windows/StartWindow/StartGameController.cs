using Base.BaseWindow;

namespace Windows.StartWindow
{
    public class StartGameController: ControllerBase<StartGameModel, StartGameView>
    {
        protected override StartGameModel CreateModel()
        {
            return new StartGameModel { IsVisible = false };
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