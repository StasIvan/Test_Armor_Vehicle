using Base.BaseWindow;

namespace Windows.LoseGameWindow
{
    public class LoseGameModel : ModelBase
    {
        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible == value) return;
                _isVisible = value;
                NotifyModelChanged();
            }
        }
    }
}