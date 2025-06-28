using Base.BaseWindow;
using Managers;

namespace Interfaces
{
    public interface IWindow
    {
        WindowType Type { get; }
        void ShowWindow();
        void HideWindow();
    }
}