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

        public virtual void Bind(TModel model)
        {
            Model = model;
        }

        public virtual void Initialize()
        {
            _transform = transform;
        }

        public virtual void Dispose()
        {
        }
        
        
    }
}