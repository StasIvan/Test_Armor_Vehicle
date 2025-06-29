using System;
using GameItems.Base;
using Zenject;

namespace Interfaces
{
    public interface IItemController : IInitializable, IDisposable
    {
        GameItemType Type { get; }
        T GetView<T>() where T : class;
    }
}