using Pool;
using UnityEngine;

namespace GameItems
{
    public abstract class BaseGameItem : MonoBehaviour
    {
        public virtual void Initialize(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}