using Cysharp.Threading.Tasks;
using Managers;

namespace Interfaces.ManagerInterfaces
{
    public interface IWindowManager
    {
        UniTask LoadWindowContent();
        void Open(WindowType type);
        void Close();
    }
}