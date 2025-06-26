using GameItems.PlayerItems;
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
            Container.DeclareSignal<ChangeGameStateSignal>();
            Container.DeclareSignal<OnDragSignal>();
            Container.DeclareSignal<OnEndDragSignal>();
            Container.DeclareSignal<OnPointerDownSignal>();
            Container.DeclareSignal<OnPointerUpSignal>();
            Container.DeclareSignal<OnContentLoadedSignal>();
            Container.DeclareSignal<OnChangePlayerStatusSignal>();
        }
    }

    public struct ChangeGameStateSignal
    {
        public GameState State;
    }

    public struct OnDragSignal
    {
        public PointerEventData EventData;
    }

    public struct OnEndDragSignal
    {
        public PointerEventData EventData;
    }

    public struct OnPointerDownSignal
    {
        public PointerEventData EventData;
    }

    public struct OnPointerUpSignal
    {
        public PointerEventData EventData;
    }

    public struct OnContentLoadedSignal { }

    public struct OnChangePlayerStatusSignal
    {
        public PlayerStatus Status;
    }
}