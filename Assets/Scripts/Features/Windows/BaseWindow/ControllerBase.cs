using System;
using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Core.Managers;

namespace Features.Windows.BaseWindow
{
    public abstract class ControllerBase<TModel, TView> : IWindow
        where TModel : ModelBase
        where TView : ViewBase<TModel>
    {
        public WindowType Type { get; }
        protected readonly TModel Model;
        protected readonly TView View;
        private readonly IWindowManager _windowManager;

        protected ControllerBase(WindowType type, TModel model, TView view, IWindowManager windowManager)
        {
            Type = type;
            Model = model ?? throw new ArgumentNullException(nameof(model));
            View = view ?? throw new ArgumentNullException(nameof(view));
            _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));

            View.Bind(Model);
        }

        public void ShowWindow()
        {
            if (Model == null)
                throw new InvalidOperationException("Model is not initialized");
            RegisterViewEvents();
            OnShow();
        }

        public void HideWindow()
        {
            OnHide();
            UnregisterViewEvents();
        }

        protected void Close()
            => _windowManager.Close();

        protected abstract void RegisterViewEvents();
        protected abstract void UnregisterViewEvents();
        protected abstract void OnShow();
        protected abstract void OnHide();
    }
}