using UnityEngine;

namespace Core.Interfaces
{
    public interface ISeekMoveController 
    {
        void SetTarget(Transform target);
        void Move();
        void Stop();
    }
}