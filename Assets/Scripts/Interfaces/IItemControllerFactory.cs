using Configs.EnemyConfig;
using GameItems.Base;
using UnityEngine;

namespace Interfaces
{
    public interface IItemControllerFactory
    {
        GameItemType Type { get; }
        IItemController Create(Vector3 position, Quaternion rotation);
        void Delete(IPool pool, IItemController controller);
    }
}