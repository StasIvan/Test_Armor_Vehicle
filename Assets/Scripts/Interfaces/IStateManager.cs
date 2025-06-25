using Managers;

namespace Interfaces
{
    public interface IStateManager
    {
        void ChangeState(GameState state);
    }
}