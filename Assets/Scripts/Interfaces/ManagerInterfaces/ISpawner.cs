using GameItems;
using GameItems.Base;
using UnityEngine;

namespace Interfaces.ManagerInterfaces
{
    public interface ISpawner
    {
        IItemController GetItem(GameItemType type, Vector3 position, Quaternion rotation);
        void ReleaseAll();
        void Release<T>(T component) where T : IItemController;
        void ReleaseAllComponents<T>() where T : class, IItemController;
    }
}