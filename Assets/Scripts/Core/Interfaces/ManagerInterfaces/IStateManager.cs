using Core.Managers;

namespace Core.Interfaces.ManagerInterfaces
{
    public interface IStateManager
    {
        GameState GameState { get; }
        void ChangeState(GameState state);
    }
}