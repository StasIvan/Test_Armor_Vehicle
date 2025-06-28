using System;
using Windows.LoseGameWindow;
using Interfaces;
using Interfaces.ManagerInterfaces;
using Managers;
using UnityEngine;

namespace Windows.ControllerFactories
{
    public class LoseGameWindowFactory : IWindowControllerFactory
    {
        public WindowType Type => WindowType.LoseGameWindow;

        public IWindow Create(GameObject windowGameObject, IWindowManager windowManager)
        {
            var view = windowGameObject.GetComponent<LoseGameView>()
                       ?? throw new InvalidOperationException(
                           $"Prefab for {Type} must have LoseGameView");
            var model = new LoseGameModel();
            return new LoseGameController(Type, model, view, windowManager);
        }
    }
}