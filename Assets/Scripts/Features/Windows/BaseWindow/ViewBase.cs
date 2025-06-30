using System;
using UnityEngine;

namespace Features.Windows.BaseWindow
{
    public abstract class ViewBase<TModel> : MonoBehaviour where TModel : ModelBase
    {
        protected TModel Model;
        private Action<ModelBase> _modelChangedCallback;

        public void Bind(TModel model)
        {
            if (Model != null && _modelChangedCallback != null)
                Model.OnModelChanged -= _modelChangedCallback;

            Model = model;

            _modelChangedCallback = baseModel =>
            {
                if (baseModel is TModel typed)
                    OnModelChanged(typed);
                else
                    Debug.LogError($"Model type mismatch: expected {typeof(TModel)}, got {baseModel.GetType()}");
            };

            Model.OnModelChanged += _modelChangedCallback;

            _modelChangedCallback(Model);
        }

        protected abstract void OnModelChanged(TModel model);

        protected virtual void OnDestroy()
        {
            if (Model != null && _modelChangedCallback != null)
                Model.OnModelChanged -= _modelChangedCallback;
        }
    }
}