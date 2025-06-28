using Windows;
using Windows.ControllerFactories;
using Interfaces;
using Managers;
using Zenject;

namespace Installers
{
    public class ManagersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SetManagers();
        }

        private void SetManagers()
        {
            Container.BindInterfacesAndSelfTo<ConfigsManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SpawnManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ContentLoaderManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CamerasManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ShootManager>().AsSingle();
            //Container.BindInterfacesAndSelfTo<WindowsManager>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<StartGameWindowFactory>()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<LoseGameWindowFactory>()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<WinGameWindowFactory>()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<WindowsManager>().AsSingle().NonLazy();
        }
    }
}