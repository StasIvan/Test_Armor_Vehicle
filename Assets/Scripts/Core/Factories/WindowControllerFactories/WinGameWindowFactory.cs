using System;
using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Core.Managers;
using Features.Windows.WinGameWindow;
using UnityEngine;

namespace Core.Factories.WindowControllerFactories
{
    public class WinGameWindowFactory : IWindowControllerFactory
    {
        public WindowType Type => WindowType.WinGameWindow;

        public IWindow Create(GameObject windowGameObject, IWindowManager windowManager)
        {
            var view = windowGameObject.GetComponent<WinGameView>()
                       ?? throw new InvalidOperationException(
                           $"Prefab for {Type} must have WinGameView");
            var model = new WinGameModel();
            return new WinGameController(Type, model, view, windowManager);
        }
    }
}