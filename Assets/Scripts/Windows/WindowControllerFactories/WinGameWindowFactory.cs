using System;
using Windows.StartWindow;
using Windows.WinGameWindow;
using Interfaces;
using Interfaces.ManagerInterfaces;
using Managers;
using UnityEngine;

namespace Windows.ControllerFactories
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