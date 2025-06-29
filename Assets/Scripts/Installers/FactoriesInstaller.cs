using Windows.ControllerFactories;
using Managers.SpawnManager.ItemControllerFactories;
using Zenject;

namespace Installers
{
    public class FactoriesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<StartGameWindowFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoseGameWindowFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<WinGameWindowFactory>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<BulletControllerFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerControllerFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyControllerFactory>().AsSingle();
        }
    }
}