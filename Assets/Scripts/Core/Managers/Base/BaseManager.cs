using System;
using Zenject;

namespace Core.Managers.Base
{
    public abstract class BaseManager : IInitializable, IDisposable
    {
        public abstract void Initialize();

        public abstract void Dispose();
    }
}
