using Managers;

namespace Interfaces
{
    public interface IWindowManager
    {
        void Open(WindowType type);
        void Close();
    }
}