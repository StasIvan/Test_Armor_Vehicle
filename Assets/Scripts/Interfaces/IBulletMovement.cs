using UnityEngine;

namespace Interfaces
{
    public interface IBulletMovement : IMovable

    {
        void SetDirection(Vector3 direction);
    }
}