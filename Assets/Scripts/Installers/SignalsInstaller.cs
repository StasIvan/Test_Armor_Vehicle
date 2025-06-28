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
            Container.DeclareSignal<OnCloseWindowSignal>();
            Container.DeclareSignal<OnOpenWindowSignal>();
            Container.DeclareSignal<OnCloseAllWindowSignal>();
            Container.DeclareSignal<OnGameItemsSpawnedSignal>();
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
    }

    public struct OnPointerDownSignal
    {
        public PointerEventData EventData;
    }

    public struct OnPointerUpSignal
    {
    }

    public struct OnContentLoadedSignal { }
    
    public struct OnGameItemsSpawnedSignal { }
    
    public struct OnChangePlayerStatusSignal
    {
        public PlayerStatus Status;
    }
    
    public struct OnCloseWindowSignal
    {
        public WindowType Type;
    }
    
    public struct OnOpenWindowSignal
    {
        public WindowType Type;
    }
    
    public struct OnCloseAllWindowSignal
    {
    }
}