using Managers;

namespace Interfaces
{
    public interface IStateManager
    {
        GameState GameState { get; }
        void ChangeState(GameState state);
    }
}