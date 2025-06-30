using System;
using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Core.Managers;
using Features.Windows.LoseGameWindow;
using UnityEngine;

namespace Core.Factories.WindowControllerFactories
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