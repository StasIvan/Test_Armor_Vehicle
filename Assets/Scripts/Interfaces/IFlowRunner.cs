using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IFlowRunner
    {
        UniTask ExecuteFlow();
    }
}