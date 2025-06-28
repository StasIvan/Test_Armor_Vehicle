using GameItems;
using Pool;
using UnityEngine;

namespace Interfaces
{
    public interface ISpawner
    {
        public T GetItem<T>(Vector3 position, Quaternion rotation) where T : BaseGameItem;

        public void ReleaseAll();
        public void Release<T>(T component) where T : BaseGameItem;
        void ReleaseAllComponents<T>() where T : Component;
    }
}