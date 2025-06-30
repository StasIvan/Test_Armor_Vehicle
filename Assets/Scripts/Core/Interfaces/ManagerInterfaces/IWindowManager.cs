using Core.Managers;
using Cysharp.Threading.Tasks;

namespace Core.Interfaces.ManagerInterfaces
{
    public interface IWindowManager
    {
        UniTask LoadWindowContent();
        void Open(WindowType type);
        void Close();
    }
}