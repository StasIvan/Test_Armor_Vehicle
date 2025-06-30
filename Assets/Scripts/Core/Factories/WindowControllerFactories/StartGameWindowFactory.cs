using System;
using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Core.Managers;
using Features.Windows.StartWindow;
using UnityEngine;

namespace Core.Factories.WindowControllerFactories
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