using Cysharp.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IHandler
    {
        UniTask Execute();
    }
}