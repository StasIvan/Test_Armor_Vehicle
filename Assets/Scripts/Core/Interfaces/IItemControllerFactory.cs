using Features.GameItems.Base;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IItemControllerFactory
    {
        GameItemType Type { get; }
        IItemController Create(Vector3 position, Quaternion rotation);
        void Delete(IPool pool, IItemController controller);
    }
}