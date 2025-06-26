using Managers;
using UnityEngine.EventSystems;
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
            Container.DeclareSignal<OnDrag>();
            Container.DeclareSignal<OnEndDrag>();
            Container.DeclareSignal<OnPointerDown>();
            Container.DeclareSignal<OnPointerUp>();
        }
    }

    public struct ChangeGameState
    {
        public GameState State;
    }
    
    public struct OnDrag
    {
        public PointerEventData EventData;
    }
    
    public struct OnEndDrag
    {
        public PointerEventData EventData;
    }
    
    public struct OnPointerDown
    {
        public PointerEventData EventData;
    }
    
    public struct OnPointerUp
    {
        public PointerEventData EventData;
    }
}