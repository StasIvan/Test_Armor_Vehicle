using System.Collections.Generic;
using Core.Pool;
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