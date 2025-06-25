using System.Collections.Generic;
using Pool;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PoolInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MultiplyPoolComponent>().FromComponentInHierarchy().AsSingle();
        }
    }
}