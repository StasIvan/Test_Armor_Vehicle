using System;
using Windows.StartWindow;
using Interfaces;
using Interfaces.ManagerInterfaces;
using Managers;
using UnityEngine;

namespace Windows.ControllerFactories
{
    public class StartGameWindowFactory : IWindowControllerFactory
    {
        public WindowType Type => WindowType.StartGameWindow;

        public IWindow Create(GameObject windowGameObject, IWindowManager windowManager)
        {
            var view = windowGameObject.GetComponent<StartGameView>()
                       ?? throw new InvalidOperationException(
                           $"Prefab for {Type} must have StartGameView");
            var model = new StartGameModel();
            return new StartGameController(Type, model, view, windowManager);
        }
    }
}