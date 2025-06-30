using System;

namespace Features.Windows.BaseWindow
{
    public abstract class ModelBase
    {
        public event Action<ModelBase> OnModelChanged;

        protected void NotifyModelChanged()
        {
            OnModelChanged?.Invoke(this);
        }
    }
}