using UnityEngine;

namespace Interfaces
{
    public interface ISeekMovement : IMovable
    {
        void SetTarget(Transform target);
    }
}