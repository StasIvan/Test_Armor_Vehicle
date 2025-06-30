using Features.GameItems;
using Features.Windows;
using Zenject;

namespace Installers
{
    public class ItemsFromInHierarchyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SetManagers();
        }

        private void SetManagers()
        {
            Container.BindInterfacesAndSelfTo<InputView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CamerasView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<UICanvas>().FromComponentInHierarchy().AsSingle();
        }
    }
}