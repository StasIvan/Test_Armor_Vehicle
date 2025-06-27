using System;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Base.BaseWindow
{
    public abstract class ControllerBase<TModel, TView> : MonoBehaviour, IWindow
        where TModel : ModelBase
        where TView : ViewBase<TModel>
    {
        [SerializeField] protected TView view;
        [SerializeField] protected WindowType windowType;
        protected TModel Model;
        private IWindowManager _windowManager;

        public WindowType Type
        {
            get => windowType;
        }
        protected void Awake()
        {
            Model = CreateModel();
            if (view == null)
                Debug.LogError($"View not assigned to Controller: {GetType().Name}");
            else
                view.Bind(Model);
        }
        
        public void ShowWindow()
        {
            if (Model is null)
                throw new InvalidOperationException("Model is not initialized");
            OnShow();
        }

        [Inject]
        public void Construct(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }
        
        public void HideWindow()
        {
            OnHide();
        }

        protected void Close()
        {
            _windowManager.Close();
        }
        
        protected void OnEnable() => RegisterViewEvents();
        protected void OnDisable() => UnregisterViewEvents();

        protected abstract TModel CreateModel();
        protected abstract void RegisterViewEvents();
        protected abstract void UnregisterViewEvents();
        
        protected abstract void OnShow();

        protected abstract void OnHide();
        
    }
}