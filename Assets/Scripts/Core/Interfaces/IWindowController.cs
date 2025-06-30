using Core.Managers;

namespace Core.Interfaces
{
    public interface IWindow
    {
        WindowType Type { get; }
        void ShowWindow();
        void HideWindow();
    }
}