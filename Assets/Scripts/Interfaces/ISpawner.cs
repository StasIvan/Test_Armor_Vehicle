using GameItems;
using Pool;
using UnityEngine;

namespace Interfaces
{
    public interface ISpawner
    {
        public T GetItem<T>(Vector3 position, Quaternion rotation) where T : BaseGameItem;

        public void ReleaseAll();
    }
}