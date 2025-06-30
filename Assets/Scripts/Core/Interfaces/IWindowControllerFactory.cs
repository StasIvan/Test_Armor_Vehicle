using Core.Interfaces.ManagerInterfaces;
using Core.Managers;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IWindowControllerFactory
    {
        WindowType Type { get; }
        IWindow Create(GameObject windowGameObject, IWindowManager windowManager);
    }
}