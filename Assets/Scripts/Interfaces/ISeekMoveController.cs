using GameItems.Base;
using UnityEngine;

namespace Interfaces
{
    public interface ISeekMoveController 
    {
        void SetTarget(Transform target);
        void Move();
        void Stop();
    }
}