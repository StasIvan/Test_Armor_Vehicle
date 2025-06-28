using System;
using Zenject;

namespace Managers.Base
{
    public abstract class BaseManager : IInitializable, IDisposable
    {
        public abstract void Initialize();

        public abstract void Dispose();
    }
}
