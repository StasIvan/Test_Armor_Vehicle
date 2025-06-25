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
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ContentLoaderManager>().AsSingle().NonLazy();
        }
    }
}