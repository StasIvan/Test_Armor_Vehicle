using UnityEngine;

namespace Interfaces
{
    public interface IBulletMovable : IMovable
    {
        void SetDirection(Vector3 direction);
    }
}