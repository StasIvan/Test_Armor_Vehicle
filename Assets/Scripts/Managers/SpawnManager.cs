using Base;
using GameItems;
using Interfaces;
using UnityEngine;

namespace Managers
{
    public class SpawnManager : BaseManager, ISpawner
    {
        public override void Initialize()
        {
        }

        public override void Dispose()
        {
        }

        public T GetItem<T>(Vector3 position, Quaternion rotation) where T : BaseGameItem
        {
            throw new System.NotImplementedException();
        }

        public void ReleaseAll()
        {
            throw new System.NotImplementedException();
        }
    }

    public enum ItemType
    {
        None,
        Player,
        Enemy,
        Bullet
    }
}