using System;
using Features.GameItems.Base;
using Zenject;

namespace Core.Interfaces
{
    public interface IItemController : IInitializable, IDisposable
    {
        GameItemType Type { get; }
        T GetView<T>() where T : class;
    }
}