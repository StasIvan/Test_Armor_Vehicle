using Managers;
using Zenject;

namespace Installers
{
    public class SignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SetSignals();
        }

        private void SetSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<ChangeGameState>();
        }
    }

    public struct ChangeGameState
    {
        public GameState State;
    }
}