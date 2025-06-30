using Cysharp.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFlowRunner
    {
        UniTask ExecuteFlow();
    }
}