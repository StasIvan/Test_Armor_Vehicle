using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IHandler
    {
        UniTask Execute();
    }
}