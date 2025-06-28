using Interfaces.ManagerInterfaces;
using Managers;
using UnityEngine;

namespace Interfaces
{
    public interface IWindowControllerFactory
    {
        WindowType Type { get; }
        IWindow Create(GameObject windowGameObject, IWindowManager windowManager);
    }
}