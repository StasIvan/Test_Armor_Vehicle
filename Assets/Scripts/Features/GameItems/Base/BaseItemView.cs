using System;
using Core.Interfaces;
using UnityEngine;
using Zenject;

namespace Features.GameItems.Base
{
    public abstract class BaseItemView<TModel> : MonoBehaviour, IItemView, IInitializable, IDisposable where TModel : BaseItemModel
    {
        [SerializeField] protected Rigidbody _rigidbody;
        protected Transform _transform;
        protected TModel Model;
        private Action<ChangedFields> _modelChangedCallback;

        public virtual void Bind(TModel model)
        {
            if (Model != null && _modelChangedCallback != null)
                Model.OnChanged -= _modelChangedCallback;

            Model = model;
            Model.OnChanged += OnModelChanged;
        }

        public virtual void Initialize()
        {
            _transform = transform;
        }

        public virtual void Dispose()
        {
            if (Model != null && _modelChangedCallback != null)
                Model.OnChanged -= _modelChangedCallback;
        }
        
        public abstract void OnModelChanged(ChangedFields field);
    }
}