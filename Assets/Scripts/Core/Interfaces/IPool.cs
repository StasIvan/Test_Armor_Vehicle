using UnityEngine;

namespace Core.Interfaces
{
    public interface IPool
    {
        T GetMonoBehaviour<T>(Vector3 position, Quaternion rotation) where T : MonoBehaviour;
        void ReleaseAll();
        void Release<T>(T component) where T : Component;
        void ReleaseAllComponents<T>() where T : Component;
        void Dispose();
    }
}